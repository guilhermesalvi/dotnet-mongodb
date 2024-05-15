using DotNetMongoDb.Conventions;
using DotNetMongoDb.EventStore;
using DotNetMongoDb.Idempotence;
using DotNetMongoDb.Indexes;
using DotNetMongoDb.Mappings;
using DotNetMongoDb.Repositories;
using DotNetMongoDb.Security;
using DotNetMongoDb.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotNetMongoDb.Extensions;

public static class DataExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddSettings()
            .AddRepositories(configuration)
            .AddIndexes();

        var encryptionKey = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!.EncryptionKey;
        ConventionsRegister.RegisterConventions(new DataSecurityService(encryptionKey));

        StoredEventMap.ConfigureMap();
        IdempotentMessageMap.ConfigureMap();

        return services;
    }

    public static void ConfigureIndexes(this IServiceProvider provider)
    {
        var idempotenceIndexes = provider.GetRequiredService<IdempotenceIndexes>();
        idempotenceIndexes.ConfigureIndexes();
    }

    private static IServiceCollection AddIndexes(this IServiceCollection services)
    {
        return services.AddSingleton<IdempotenceIndexes>();
    }

    private static IServiceCollection AddSettings(this IServiceCollection services)
    {
        services
            .AddOptions<MongoDbSettings>()
            .BindConfiguration(nameof(MongoDbSettings))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.BuildServiceProvider().GetRequiredService<IOptionsMonitor<MongoDbSettings>>().Get(nameof(MongoDbSettings));

        return services;
    }

    private static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>()!;

        services.AddSingleton<IMongoClient, MongoClient>(_ =>
            new MongoClient(MongoClientSettings.FromConnectionString(settings.ConnectionString)));

        services.AddSingleton<IMongoDatabase>(sp =>
            sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));

        services.AddScoped<IEventStore, Repositories.EventStore>();
        services.AddScoped<IIdempotentReceiver, IdempotentReceiver>();

        return services;
    }
}
