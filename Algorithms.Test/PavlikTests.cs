namespace AmbientBytes.Algorithms.Test;

[TestFixture]
public class PavlikTests
{
    private IDataProcessor _proccessor;

    [SetUp]
    public void Setup()
    {
        _proccessor = new Pavlik();
    }

    [TestCase(new[] { 1L, 2L, 3L, 4L }, new long[0], 1L, IDataProcessor.Order.Ascending, 10, new[] { 2L, 3L, 4L })]
    [TestCase(new[] { 1L, 2L }, new[] { 3L, 4L }, 1L, IDataProcessor.Order.Ascending, 10, new[] { 2L, 3L, 4L })]
    [TestCase(new[] { 1L, 2L, 3L, 4L }, new[] { 1L, 2L, 3L, 4L }, 1L, IDataProcessor.Order.Ascending, 10, new[] { 2L, 3L, 4L })]
    [TestCase(new[] { 1L, 2L, 3L, 4L }, new long[0], 2L, IDataProcessor.Order.Ascending, 10, new[] { 3L, 4L })]
    [TestCase(new[] { 1L, 2L, 3L, 4L }, new long[0], 2L, IDataProcessor.Order.Descending, 10, new[] { 1L })]
    [TestCase(new[] { 1L, 2L, 3L, 4L }, new long[0], 5L, IDataProcessor.Order.Descending, 3, new[] { 4L, 3L, 2L })]
    [TestCase(new[] { 1L, 2L, 3L, 4L }, new long[0], 5L, IDataProcessor.Order.Descending, 10, new[] { 4L, 3L, 2L, 1L })]
    [TestCase(new[] { 1L, 1L, 1L, 1L }, new[] { 1L, 1L, 1L }, 0L, IDataProcessor.Order.Ascending, 10, new[] { 1L })]
    [TestCase(new[] { 1L, 1L, 1L, 1L }, new[] { 1L, 1L, 1L }, 2L, IDataProcessor.Order.Descending, 10, new[] { 1L })]
    public void ValidInput_CorrectOutput(long[] input, long[] existing, long start, IDataProcessor.Order order, int count, long[] expected)
    {
        var sequence = _proccessor.ProcessData(
            MakeList(input),
            start,
            order,
            count,
            MakeList(existing));

        Assert.That(from e in sequence select e.Id, Is.EquivalentTo(expected));
    }

    private static List<Entity> MakeList(params long[] ids)
    {
        List<Entity> list = new List<Entity>(ids.Length);

        foreach (var id in ids)
        {
            list.Add(new Entity(id, id * id, DateTime.Now));
        }

        return list;
    }
}