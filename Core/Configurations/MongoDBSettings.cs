namespace Core.Configurations
{
    public class MongoDBSettings : IMongoDBSettings
    {
        public required string Host { get; set; }

        public required string Database { get; set; }

        public string ConnectionString => $"{Host}";

    }

    public interface IMongoDBSettings
    {
        string Host { get; set; }
        string Database { get; set; }
        string ConnectionString => $"{Host}";
    }
}
