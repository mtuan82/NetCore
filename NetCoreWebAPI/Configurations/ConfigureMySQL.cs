using Core.Configurations;
using Core.Providers.MySQL;
using Microsoft.EntityFrameworkCore;
using NetCoreWebAPI.Services.MySQL;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMySQL
    {
        public static void SetupDatabase(this IServiceCollection services, MySQLSettings databaseSettings)
        {
            services.AddDbContext<MySQLProvider>(options =>
                options.UseLazyLoadingProxies().UseMySQL(databaseSettings.ConnectionString)
             , ServiceLifetime.Singleton);

            services.AddScoped<IMySQLService, MySQLService>();
        }
    }
}
