using System.Buffers;

namespace AmbientBytes.Algorithms;

public sealed class Pavlik : IDataProcessor
{
    IEnumerable<Entity> IDataProcessor.ProcessData(List<Entity> input, long startAfterId, IDataProcessor.Order order, int count, List<Entity> existingData)
    {
        int dataLength = input.Count + existingData.Count;
        Entity[] pooledArray = ArrayPool<Entity>.Shared.Rent(dataLength);

        try
        {
            // Copy both lists to the array and sort the combined data by the requested order.
            input.CopyTo(pooledArray, 0);
            existingData.CopyTo(pooledArray, input.Count);
            Array.Sort(pooledArray, 0, dataLength, EntityComparerForOrder(order));
            var workData = new ArraySegment<Entity>(pooledArray, 0, input.Count + existingData.Count);
            
            // Find the index all elements to the right of that are after the starting point
            int nextIndex = FindNextInOrder(workData, startAfterId, IdComparerForOrder(order));

            if (nextIndex >= workData.Count)
            {
                return Array.Empty<Entity>();
            }

            int deduplicatedLength = Deduplicate(workData, nextIndex + 1);

            if (deduplicatedLength == 0)
            {
                return Array.Empty<Entity>();
            }

            return workData.Slice(nextIndex + 1, deduplicatedLength > count ? count : deduplicatedLength).ToArray();
        }
        finally
        {
            ArrayPool<Entity>.Shared.Return(pooledArray, true);
        }
    }

    private static int FindNextInOrder(ArraySegment<Entity> data, long id, IComparer<long> order)
    {
        int lb = -1;
        int ub = data.Count;

        while (ub - lb > 1)
        {
            int middle = (lb + ub) / 2;

            if (order.Compare(data[middle].Id, id) <= 0)
            {
                lb = middle;
            }
            else
            {
                ub = middle;
            }
        }

        return lb;
    }

    private static int Deduplicate(ArraySegment<Entity> data, int start)
    {
        if (start >= data.Count)
        {
            return 0;
        }
        
        int w = 0;
        
        for (int r = start + 1; r < data.Count; ++r)
        {
            if (data[r].Id != data[start + w].Id)
            {
                if (++w != r)
                {
                    data[start + w] = data[r];
                }
            }
        }

        return w + 1;
    }

    private static IComparer<Entity> EntityComparerForOrder(IDataProcessor.Order order) =>
        order == IDataProcessor.Order.Descending ? DescendingEntityOrder.Comparer : AscendingEntityOrder.Comparer;

    private static IComparer<long> IdComparerForOrder(IDataProcessor.Order order) =>
        order == IDataProcessor.Order.Descending ? DescendingIdOrder.Comparer : AscendingIdOrder.Comparer;
    
    private sealed class AscendingEntityOrder : IComparer<Entity>
    {
        public static readonly IComparer<Entity> Comparer = new AscendingEntityOrder();
        int IComparer<Entity>.Compare(Entity? x, Entity? y) => x.Id.CompareTo(y.Id);
    }

    private sealed class DescendingEntityOrder : IComparer<Entity>
    {
        public static readonly IComparer<Entity> Comparer = new DescendingEntityOrder();
        int IComparer<Entity>.Compare(Entity? x, Entity? y) => y.Id.CompareTo(x.Id);
    }
    
    private sealed class AscendingIdOrder : IComparer<long>
    {
        public static readonly IComparer<long> Comparer = new AscendingIdOrder();
        int IComparer<long>.Compare(long x, long y) => x.CompareTo(y);
    }

    private sealed class DescendingIdOrder : IComparer<long>
    {
        public static readonly IComparer<long> Comparer = new DescendingIdOrder();
        int IComparer<long>.Compare(long x, long y) => y.CompareTo(x);
    }
}