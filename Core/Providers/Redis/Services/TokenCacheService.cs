using Core.Providers.Redis.Entity;
using Core.Providers.Redis.Services.Interface;

namespace Core.Providers.Redis.Services
{
    public class TokenCacheService : ITokenCacheService
    {
        private readonly IRedisProvider _cacheClient;

        private const string RootKey = "Token";

        public TokenCacheService(IRedisProvider cacheClient)
        {
            _cacheClient = cacheClient;
        }

        public async Task<IEnumerable<Token>> Get(string key)
        {
            var operation = new Func<Task<IEnumerable<Token>>>(
                async () =>
                {
                    var samplePrices = await _cacheClient.GetValue<Token>(RootKey, key);

                    return samplePrices;
                }
            );

            return await RunCachingOperation(operation);
        }

        public async Task CreateOrUpdate(IEnumerable<Token> samplePrices, string key)
        {
            if (samplePrices != null)
            {
                var operation = new Func<Task<object>>(
                    async () =>
                    {
                        await _cacheClient.SetValue(RootKey, samplePrices, key);
                        return null;
                    }
                );

                await RunCachingOperation(operation);
            }
        }

        private static async Task<T> RunCachingOperation<T>(Func<Task<T>> cachingOperation, int attempts = 0)
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

        public async Task Delete(string key)
        {
            var operation = new Func<Task>(
                async () =>
                {
                    await _cacheClient.DeleteValue(RootKey, key);
                }
            );

            //await RunCachingOperation(operation);
        }
    }
}
