using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Core.Providers.MongoDB
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedDate { get; }
    }
}
