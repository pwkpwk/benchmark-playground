namespace AmbientBytes.Algorithms;

public interface IDataProcessor
{
    enum Order
    {
        Ascending,
        Descending
    }

    /// <summary>
    /// Return a collection of elements from <see cref="input"/> and <see cref="existingData"/> sorted by <see cref="Entity.Id"/>
    /// in the specified order and with duplicate values of <see cref="Entity.Id"/> removed.
    /// </summary>
    IEnumerable<Entity> ProcessData(List<Entity> input, long startAfterId, Order order, int count, List<Entity> existingData);
}