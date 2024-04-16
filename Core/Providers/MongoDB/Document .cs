using MongoDB.Bson;

namespace Core.Providers.MongoDB
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedDate => Id.CreationTime;
    }
}
