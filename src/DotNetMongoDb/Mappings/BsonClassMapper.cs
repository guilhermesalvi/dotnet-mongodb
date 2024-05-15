using MongoDB.Bson.Serialization;

namespace DotNetMongoDb.Mappings;

public static class BsonClassMapper
{
    private static readonly object Lock = new();

    public static void Register<T>(Action<BsonClassMap<T>> classMapInitializer)
    {
        lock (Lock)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap(classMapInitializer);
            }
        }
    }
}