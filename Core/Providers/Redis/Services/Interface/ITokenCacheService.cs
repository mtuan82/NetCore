using Core.Providers.Redis.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Providers.Redis.Services.Interface
{
    public interface ITokenCacheService
    {
        public Task<IEnumerable<Token>> Get(string key);
        public Task CreateOrUpdate(IEnumerable<Token> samplePrices, string key);
        public Task Delete(string key);
    }
}
