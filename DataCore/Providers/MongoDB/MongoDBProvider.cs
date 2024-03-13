using DataCore.Providers.MongoDB.Entity;
using MongoDB.Driver;

namespace DataCore.Providers.MongoDB
{
    public class MongoDBProvider
    {
        private readonly IMongoDatabase _database;

        public MongoDBProvider(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    }
}
