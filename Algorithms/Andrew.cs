namespace AmbientBytes.Algorithms;

public sealed class Andrew : IDataProcessor
{
    IEnumerable<Entity> IDataProcessor.ProcessData(List<Entity> input, long startAfterId, IDataProcessor.Order order, int count, List<Entity> existingData)
    {
        var response = order == IDataProcessor.Order.Ascending
            ? input.Where(x => x.Id > startAfterId).ToList()
            : input.Where(x => x.Id < startAfterId).ToList();

        response.RemoveAll(item => existingData.Any(x => item.RandomValue == x.RandomValue));

        response = order == IDataProcessor.Order.Ascending
            ? response.OrderBy(x => x.Id).Take(count).ToList()
            : response.OrderByDescending(x => x.Id).Take(count).ToList();

        var output = new List<Entity>(input.Count + existingData.Count);
        output.AddRange(existingData);
        output.AddRange(response);

        return output;
    }
}