using DotNetMongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DotNetMongoDb.EventStore;

namespace DotNetMongoDb.Repositories;

public class EventStore(
    IMongoDatabase database,
    IOptions<MongoDbSettings> settings) : IEventStore
{
    private readonly IMongoCollection<StoredEvent> _collection = database
        .GetCollection<StoredEvent>(settings.Value.EventStoreCollectionName);

    public Task AppendAsync(StoredEvent storedEvent, CancellationToken cancellationToken) =>
        _collection.InsertOneAsync(storedEvent, cancellationToken: cancellationToken);

    public IEnumerable<StoredEvent> FetchAsync(string eventType, CancellationToken cancellationToken)
    {
        var filter = Builders<StoredEvent>.Filter.Eq(x => x.EventType, eventType);
        return _collection.Find(filter).ToEnumerable(cancellationToken: cancellationToken);
    }
}
