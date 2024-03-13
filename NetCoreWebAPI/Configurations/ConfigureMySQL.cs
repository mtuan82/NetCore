using DataCore.Configurations;
using DataCore.Providers.MySQL;
using Microsoft.EntityFrameworkCore;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMySQL
    {
        public static void SetupDatabase(this IServiceCollection services, MySQLSettings databaseSettings)
        {
            services.AddDbContext<MySQLProvider>(options =>
                options.UseMySQL(databaseSettings.ConnectionString)
             , ServiceLifetime.Singleton);
        }
    }
}
