using Core.Providers.Redis;
using NetCoreWebAPI.Services.Redis.Model;

namespace NetCoreWebAPI.Services.Redis
{
    public class RedisService: IRedisService
    {
        private readonly IRedisProvider _cacheClient;

        private const string RootKey = "Price";

        public RedisService(IRedisProvider cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public async Task<IEnumerable<Price>> Get(string key)
        {
            var operation = new Func<Task<IEnumerable<Price>>>(
                async () =>
                {
                    var samplePrices = await _cacheClient.GetValue<Price>(RootKey, key);

                    return samplePrices;
                }
            );

            return await RunCachingOperation(operation);
        }

        public async Task CreateOrUpdate(IEnumerable<Price> samplePrices, string key)
        {
            if (samplePrices != null)
            {
                var operation = new Func<Task<bool>>(
                    async () =>
                    {
                        await _cacheClient.SetValue(RootKey, samplePrices, key);
                        return true;
                    }
                );

                await RunCachingOperation(operation);
            }
        }

        public async Task Delete(string key)
        {
            var operation = new Func<Task<bool>>(
                async () =>
                {
                    await _cacheClient.DeleteValue(RootKey, key);
                    return true;
                }
            );

            await RunCachingOperation(operation);
        }

        private async Task<T> RunCachingOperation<T>(Func<Task<T>> cachingOperation, int attempts = 0)
        {
            try
            {
                return await cachingOperation();
            }
            catch (Exception)
            {
                if (attempts < 3)
                {
                    return await RunCachingOperation(cachingOperation, ++attempts);
                }

                throw;
            }
        }
    }
}
