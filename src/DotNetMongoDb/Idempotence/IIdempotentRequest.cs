namespace DotNetMongoDb.Idempotence;

public interface IIdempotentRequest
{
    string Key { get; set; }
    bool ForceExecution { get; set; }
}