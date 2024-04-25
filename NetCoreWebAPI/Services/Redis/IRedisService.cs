using NetCoreWebAPI.Services.Redis.Model;

namespace NetCoreWebAPI.Services.Redis
{
    public interface IRedisService
    {
        Task<IEnumerable<Price>> Get(string key);
        Task CreateOrUpdate(IEnumerable<Price> samplePrices, string key);
        Task Delete(string key);
    }
}
