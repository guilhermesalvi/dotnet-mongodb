using DotNetMongoDb.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using DotNetMongoDb.Idempotence;

namespace DotNetMongoDb.Indexes;

public sealed class IdempotenceIndexes(IServiceProvider provider) : IDisposable
{
    private readonly IServiceScope _scope = provider.CreateScope();

    private bool _disposed;

    public void ConfigureIndexes()
    {
        var database = _scope.ServiceProvider.GetRequiredService<IMongoDatabase>();
        var settings = _scope.ServiceProvider.GetRequiredService<IOptions<MongoDbSettings>>();

        var collection = database.GetCollection<IdempotentMessage>(settings.Value.IdempotenceCollectionName);

        collection.Indexes.CreateOne(new CreateIndexModel<IdempotentMessage>(
            Builders<IdempotentMessage>.IndexKeys.Ascending(nameof(IdempotentMessage.Timestamp)),
            new CreateIndexOptions { Name = "timestamp_ttl", ExpireAfter = TimeSpan.FromDays(15) }));
    }

    public void Dispose()
    {
        if (_disposed) return;

        _scope.Dispose();
        _disposed = true;
    }
}
