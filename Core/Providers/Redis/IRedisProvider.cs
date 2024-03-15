namespace Core.Providers.Redis
{
    public interface IRedisProvider
    {
        Task<IEnumerable<T>> GetValue<T>(string rootKey, string itemKey) where T : class, new();

        Task SetValue<T>(string rootKey, IEnumerable<T> values, string key) where T : class, new();

        Task DeleteValue(string rootKey, string key);
    }
}