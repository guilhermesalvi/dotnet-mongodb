namespace DotNetMongoDb.Idempotence;

public record IdempotentMessage(string Key)
{
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}