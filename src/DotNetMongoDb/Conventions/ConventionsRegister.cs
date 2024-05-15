using DotNetMongoDb.Security;
using MongoDB.Bson.Serialization.Conventions;

namespace DotNetMongoDb.Conventions;

public static class ConventionsRegister
{
    public static void RegisterConventions(DataSecurityService dataSecurityService)
    {
        var pack = new ConventionPack
        {
            new GuidAsStringConvention(),
            new ProtectedPersonalDataConvention(dataSecurityService),
            new UtcDateTimeConvention()
        };

        ConventionRegistry.Register(
            "Global Conventions",
            pack,
            filter => filter.Namespace!.StartsWith("DotNetMongoDb"));
    }
}
