using Core.Configurations;
using Core.Providers.MySQL;
using Core.Providers.MySQL.Entity;
using Microsoft.EntityFrameworkCore;
using NetCoreWebAPI.Services.MySQL;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMySQL
    {
        public static void SetupDatabase(this IServiceCollection services, MySQLSettings databaseSettings)
        {
            services.AddDbContext<MySQLContext>(options =>
                options.UseLazyLoadingProxies().UseMySQL(databaseSettings.ConnectionString)
             , ServiceLifetime.Singleton);

            //inject services
            services.AddTransient<IMySQLService, MySQLService>();

            //mapping enity
            services.AddAutoMapper(c =>
            {
                c.CreateMap<Customer, Customer>();
            });
            
        }
    }
}
