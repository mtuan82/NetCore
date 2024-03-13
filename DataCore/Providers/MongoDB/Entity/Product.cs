using global::MongoDB.Bson.Serialization.Attributes;
using global::MongoDB.Bson;

namespace DataCore.Providers.MongoDB.Entity
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
