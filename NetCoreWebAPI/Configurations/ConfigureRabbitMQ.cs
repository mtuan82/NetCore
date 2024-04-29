using Core.Configurations;
using Core.Providers.RabbitMQ;
using NetCoreWebAPI.Services.RabbitMQ;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureRabbitMQ
    {
        public static void SetupRabbitMQ(this IServiceCollection services, RabbitMQSettings settings)
        {
            services.AddSingleton<IRabbitMQProvider, RabbitMQProvider>(option => new RabbitMQProvider(settings));

            services.AddScoped<IRabbitMQService, RabbitMQService>();
        }
    }
}
