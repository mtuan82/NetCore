using Core.Configurations;
using Core.Providers.MSSQL;
using Core.Providers.MSSQL.Entity;
using Microsoft.EntityFrameworkCore;
using NetCoreWebAPI.Services.MSSQL;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureMSSQL
    {
        public static void SetupDatabase(this IServiceCollection services, MSSQLSettings databaseSettings)
        {
            services.AddDbContext<MSSQLContext>(option =>
            {
                option.UseLazyLoadingProxies()
                        .UseSqlServer(databaseSettings.ConnectionString);
            }, ServiceLifetime.Scoped);

            //inject services
            services.AddTransient<IMSSQLService, MSSQLService>();

            //mapping enity
            services.AddAutoMapper(c =>
            {
                c.CreateMap<Store, Store>();
            });
        }
    }
}
