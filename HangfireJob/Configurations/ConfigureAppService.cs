using HangfireJob.ApplicationLogic;

namespace HangfireJob.Configurations
{
    public static class ConfigureAppService
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ISendMailService, SendMailService>();
        }
    }
}
