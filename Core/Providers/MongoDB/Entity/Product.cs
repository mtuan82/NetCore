using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Core.Providers.MongoDB.Entity
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Category")]
        public string Category { get; set; }

        [BsonElement("Price")]
        public decimal Price { get; set; }
    }

}
