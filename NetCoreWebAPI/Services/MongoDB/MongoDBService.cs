using Core.Providers.MongoDB;
using NetCoreWebAPI.Services.MongoDB.Model;

namespace NetCoreWebAPI.Services.MongoDB
{
    public class MongoDBService : IMongoDBService
    {
        private readonly IMongodbRepository<Product> _productRepository;

        public MongoDBService(IMongodbRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task InsertOne(Product prod)
        {
            await _productRepository.InsertOneAsync(prod);
        }

        public async Task UpdateOne(Product prod)
        {
            await _productRepository.ReplaceOneAsync(prod);
        }

        public async Task DeleteOne(string id)
        {
            await _productRepository.DeleteByIdAsync(id);
        }

        public async Task<Product> FindById(string id)
        {
            return await _productRepository.FindByIdAsync(id);
        }

        public IEnumerable<Product> FindContainsName(string name)
        {
            return _productRepository.FilterBy(x => x.Name.Contains(name));
        }
    }


    public interface IMongoDBService
    {
        Task InsertOne(Product prod);
        Task UpdateOne(Product prod);
        Task DeleteOne(string id);
        Task<Product> FindById(string id);
        IEnumerable<Product> FindContainsName(string name);
    }
}
