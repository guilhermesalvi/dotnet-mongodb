namespace DotNetMongoDb.EventStore;

public interface IEventStore
{
    IEnumerable<StoredEvent> FetchAsync(string eventType, CancellationToken cancellationToken);
    Task AppendAsync(StoredEvent storedEvent, CancellationToken cancellationToken);
}