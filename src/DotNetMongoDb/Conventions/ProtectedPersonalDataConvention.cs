using DotNetMongoDb.Attributes;
using DotNetMongoDb.Security;
using DotNetMongoDb.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace DotNetMongoDb.Conventions;

public class ProtectedPersonalDataConvention(DataSecurityService service)
    : ConventionBase, IMemberMapConvention
{
    public void Apply(BsonMemberMap memberMap)
    {
        if (memberMap.MemberType == typeof(string) &&
            Attribute.IsDefined(memberMap.MemberInfo, typeof(ProtectedPersonalDataAttribute)))
        {
            memberMap.SetSerializer(new EncryptedStringSerializer(service));
        }
    }
}
