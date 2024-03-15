
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;
using Newtonsoft.Json;

namespace Core.Providers.Redis
{
    public class RedisProvider : IRedisProvider
    {
        private readonly IRedisClient _redisClient;

        private const string DefaultCacheKey = "";

        public RedisProvider(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        public int CacheDurationMinutes { get; set; } = 60;

        public async Task DeleteValue(string rootKey, string key)
        {
            await Redis().KeyDeleteAsync($"{rootKey ?? DefaultCacheKey}-{key.ToLowerInvariant()}");
        }

        public async Task<IEnumerable<T>> GetValue<T>(string rootKey, string itemKey) where T : class, new()
        {
            if (itemKey != null)
            {
                var key = $"{rootKey ?? DefaultCacheKey}-{itemKey.ToLowerInvariant()}";

                var value = await Redis().StringGetAsync(key);

                if (value != RedisValue.Null)
                {
                    return JsonConvert.DeserializeObject<IEnumerable<T>>(value);
                }
            }

            return null;
        }

        public async Task SetValue<T>(string rootKey, IEnumerable<T> valuesArray, string key)
            where T : class, new()
        {
            if (valuesArray == null)
            {
                throw new ArgumentNullException(nameof(valuesArray));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            await Redis().StringSetAsync(new RedisKey($"{rootKey ?? DefaultCacheKey}-{key.ToLowerInvariant()}"), new RedisValue(JsonConvert.SerializeObject(valuesArray)), new TimeSpan(0, 0, CacheDurationMinutes, 0));
        }

        private IDatabase Redis()
        {
            return _redisClient.GetDefaultDatabase().Database;
        }
    }
}
