using Core.Configurations;
using Core.Providers.PostgreSQL;
using Core.Providers.PostgreSQL.Entity;
using Microsoft.EntityFrameworkCore;
using NetCoreWebAPI.Services.PostgreSQL;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigurePostgreSQL
    {
        public static void SetupDatabase(this IServiceCollection services, PostgreSQLSettings databaseSettings)
        {
            services.AddDbContext<PostgreSQLContext>(option =>
            {
                option.UseLazyLoadingProxies()
                        .UseNpgsql(databaseSettings.ConnectionString);
            }, ServiceLifetime.Singleton);

            //inject services
            services.AddTransient<IPostgreSQLService, PostgreSQLService>();

            //mapping enity
            services.AddAutoMapper(c =>
            {
                c.CreateMap<Store, Store>();
            });
        }
    }
}
