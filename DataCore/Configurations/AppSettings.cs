using Microsoft.Extensions.Configuration;

namespace DataCore.Configurations
{
    public class AppSettings
    {
        private readonly IConfiguration Configuration;
        public AppSettings(IConfiguration configuration) {
            Configuration = configuration;
        }
        public MSSQLSettings MssqlDB { get; set; }
        public MySQLSettings MysqlDB { get; set; }
        public PostgreSQLSettings PostgresqlDB { get; set; }
        public MongoDBSettings MongoDB { get; set; }
        public RedisSettings Redis { get; set; }

        public void SetupEnv()
        {
            MssqlDB = new MSSQLSettings()
            {
                Port = Configuration["MssqlDB:Port"]??"",
                Host = Configuration["MssqlDB:Host"]??"",
                Database = Configuration["MssqlDB:Database"]??"",
                UserName = Configuration["MssqlDB:UserName"]??"",
                Password = Configuration["MssqlDB:Password"]??"",
            };

            MysqlDB = new MySQLSettings()
            {
                Port = Configuration["Mysqldb:Port"] ?? "",
                Host = Configuration["Mysqldb:Host"] ?? "",
                Database = Configuration["Mysqldb:Database"] ?? "",
                UserName = Configuration["Mysqldb:UserName"] ?? "",
                Password = Configuration["Mysqldb:Password"] ?? "",
            };

            PostgresqlDB = new PostgreSQLSettings()
            {
                Port = Configuration["Postgresqldb:Port"] ?? "",
                Host = Configuration["Postgresqldb:Host"] ?? "",
                Database = Configuration["Postgresqldb:Database"] ?? "",
                UserName = Configuration["Postgresqldb:UserName"] ?? "",
                Password = Configuration["Postgresqldb:Password"] ?? "",
            };

            MongoDB = new MongoDBSettings()
            {
                Port = Configuration["Mongodb:Port"] ?? "",
                Host = Configuration["Mongodb:Host"] ?? "",
                Database = Configuration["Mongodb:Database"] ?? "",
                UserName = Configuration["Mongodb:UserName"] ?? "",
                Password = Configuration["Mongodb:Password"] ?? "",
            };

            Redis = new RedisSettings()
            {
                Host = Configuration["Redis:Host"] ?? "",
                Port = Configuration["Redis:Port"] == null ? 6379:int.Parse(Configuration["Redis:Port"]),
                PoolSize = Configuration["Redis:PoolSize"] == null ? 1: int.Parse(Configuration["Redis:PoolSize"])
            };
        }
        
    }
}
