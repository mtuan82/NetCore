using Core.Providers.MongoDB;

namespace NetCoreWebAPI.Services.MongoDB.Model
{
    [BsonCollection("Products")]
    public class Product : Document
    {
        public required string Name { get; set; }
        public double Price { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
