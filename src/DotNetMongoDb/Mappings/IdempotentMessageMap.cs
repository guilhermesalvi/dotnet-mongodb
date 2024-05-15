using DotNetMongoDb.Idempotence;

namespace DotNetMongoDb.Mappings;

public static class IdempotentMessageMap
{
    public static void ConfigureMap()
    {
        BsonClassMapper.Register<IdempotentMessage>(cm =>
        {
            cm.AutoMap();
        });
    }
}
