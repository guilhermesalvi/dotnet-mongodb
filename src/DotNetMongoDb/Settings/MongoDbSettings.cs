using System.ComponentModel.DataAnnotations;

namespace DotNetMongoDb.Settings;

public class MongoDbSettings
{
    [Required(AllowEmptyStrings = false)]
    public required string ConnectionString { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string DatabaseName { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string EventStoreCollectionName { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string IdempotenceCollectionName { get; init; }

    [Required(AllowEmptyStrings = false)]
    public required string EncryptionKey { get; init; }
}
