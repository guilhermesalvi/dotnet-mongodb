using DotNetMongoDb.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DotNetMongoDb.Idempotence;

namespace DotNetMongoDb.Repositories;

public class IdempotentReceiver(
    IMongoDatabase database,
    IOptions<MongoDbSettings> settings) : IIdempotentReceiver
{
    private readonly IMongoCollection<IdempotentMessage> _collection = database
        .GetCollection<IdempotentMessage>(settings.Value.IdempotenceCollectionName);

    public async Task<bool> IsProcessedAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var filter = Builders<IdempotentMessage>.Filter.Eq(nameof(IdempotentMessage.Key), key);

        return await _collection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) is not null;
    }

    public Task SetProcessedAsync(
        IdempotentMessage message,
        CancellationToken cancellationToken)
    {
        return _collection.InsertOneAsync(message, cancellationToken: cancellationToken);
    }

    public Task SetUnprocessedAsync(
        string key,
        CancellationToken cancellationToken)
    {
        var filter = Builders<IdempotentMessage>.Filter.Eq(nameof(IdempotentMessage.Key), key);
        return _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken);
    }
}
