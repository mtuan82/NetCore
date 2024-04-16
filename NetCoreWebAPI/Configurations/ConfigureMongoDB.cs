using Core.Configurations;
using Core.Providers.MongoDB;
using Microsoft.Extensions.Options;
using NetCoreWebAPI.Services.MongoDB;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMongoDB
    {
        public static void SetupDatabase(this IServiceCollection services, IConfiguration databaseSettings)
        {
            services.Configure<MongoDBSettings>(databaseSettings.GetSection("Mongodb"));
            services.AddSingleton<IMongoDBSettings>(serviceProvider => serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value);
            services.AddScoped(typeof(IMongodbRepository<>), typeof(MongodbRepository<>));

            services.AddScoped<IMongoDBService,MongoDBService>();
        }
    }
}
