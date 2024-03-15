using Core.Configurations;
using Core.Providers.MongoDB;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMongoDB
    {
        public static void SetupDatabase(this IServiceCollection services, MongoDBSettings databaseSettings) =>
            services.AddSingleton<MongoDBProvider>(options => { return new MongoDBProvider(databaseSettings.ConnectionString, databaseSettings.Database); });

    }
}
