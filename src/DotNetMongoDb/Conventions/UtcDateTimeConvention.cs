using DotNetMongoDb.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;

namespace DotNetMongoDb.Conventions;

public class UtcDateTimeConvention : ConventionBase, IMemberMapConvention
{
    public void Apply(BsonMemberMap memberMap)
    {
        if (memberMap.MemberType == typeof(DateTime))
        {
            memberMap.SetSerializer(new UtcDateTimeSerializer());
        }
        else if (memberMap.MemberType == typeof(DateTime?))
        {
            memberMap.SetSerializer(new NullableSerializer<DateTime>(new UtcDateTimeSerializer()));
        }
    }
}