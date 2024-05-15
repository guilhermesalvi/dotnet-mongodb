using DotNetMongoDb.EventStore;

namespace DotNetMongoDb.Mappings;

public static class StoredEventMap
{
    public static void ConfigureMap()
    {
        BsonClassMapper.Register<StoredEvent>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(x => x.Id);
        });
    }
}