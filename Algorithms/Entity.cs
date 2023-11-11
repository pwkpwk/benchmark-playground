namespace AmbientBytes.Algorithms;

public sealed class Entity
{
    public long Id { get; }
    public long RandomValue { get; }
    public DateTime Timestamp { get; }

    public Entity(long id, long randomValue, DateTime timestamp)
    {
        Id = id;
        RandomValue = randomValue;
        Timestamp = timestamp;
    }
}