namespace DotNetMongoDb.Idempotence;

public interface IIdempotentReceiver
{
    Task<bool> IsProcessedAsync(string key, CancellationToken cancellationToken);
    Task SetProcessedAsync(IdempotentMessage message, CancellationToken cancellationToken);
    Task SetUnprocessedAsync(string key, CancellationToken cancellationToken);
}