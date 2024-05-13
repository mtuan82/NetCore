using Core.Configurations;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureEncryption
    {
        public static void SetupEncryption(this IServiceCollection services, IConfiguration _settings)
        {
            services.Configure<EncryptionSettings>(_settings.GetSection("Encryption"));
        }
    }
}
