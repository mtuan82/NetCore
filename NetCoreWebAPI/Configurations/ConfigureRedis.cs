using Core.Configurations;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureRedis
    {
        public static void RegisterRedis(this IServiceCollection services, RedisSettings settings)
        {
            var config = new RedisConfiguration
            {
                SyncTimeout = 10000,
                PoolSize = settings.PoolSize,
                Hosts = new[]
                {
                    new RedisHost
                    {
                        Host = settings.Host,
                        Port = Convert.ToInt32(settings.Port)
                    }
                }
            };
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(config);
        }
    }
}
