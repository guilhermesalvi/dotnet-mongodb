namespace DotNetMongoDb.EventStore;

public record StoredEvent(
    Guid Id,
    Guid CorrelationId,
    Guid AggregateId,
    string EventType,
    string SerializedData)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}