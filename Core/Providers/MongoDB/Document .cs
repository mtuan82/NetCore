using MongoDB.Bson;

namespace Core.Providers.MongoDB
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }
        public required string UserId { get; set; }
        public DateTime CreatedDate => Id.CreationTime;
        public required string LastUpdateBy { get; set; }
    }
}
