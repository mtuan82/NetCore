using Microsoft.Extensions.Configuration;

namespace Core.Configurations
{
    public class AppSettings
    {
        private readonly IConfiguration Configuration;
        public AppSettings(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IdentitySettings Identity { get; set; }
        public MSSQLSettings MssqlDB { get; set; }
        public MySQLSettings MysqlDB { get; set; }
        public PostgreSQLSettings PostgresqlDB { get; set; }
        public RedisSettings Redis { get; set; }
        public RabbitMQSettings RabbitMQ { get; set; }

        public void SetupEnv()
        {
            Identity = new IdentitySettings()
            {
                IsLocal = bool.Parse(Configuration["Identity:IsLocal"] ?? "true") ,
                SigningKey = Configuration["Identity:SigningKey"] ?? "",
                Issuers = Configuration["Identity:Issuers"] ?? "",
                IdentityDomain = Configuration["Identity:IdentityDomain"] ?? "",
                ValidateHttps = bool.Parse(Configuration["Identity:ValidateHttps"] ?? "false")
            };

            MssqlDB = new MSSQLSettings()
            {
                Port = Configuration["MssqlDB:Port"] ?? "",
                Host = Configuration["MssqlDB:Host"] ?? "",
                Database = Configuration["MssqlDB:Database"] ?? "",
                UserName = Configuration["MssqlDB:UserName"] ?? "",
                Password = Configuration["MssqlDB:Password"] ?? "",
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

            Redis = new RedisSettings()
            {
                Host = Configuration["Redis:Host"] ?? "",
                Port = Configuration["Redis:Port"] == null ? 6379 : int.Parse(Configuration["Redis:Port"]),
                PoolSize = Configuration["Redis:PoolSize"] == null ? 1 : int.Parse(Configuration["Redis:PoolSize"])
            };

            RabbitMQ = new RabbitMQSettings()
            {
                Host = Configuration["RabbitMQ:Host"] ?? "",
                Port = Configuration["RabbitMQ:Port"] == null ? 6379 : int.Parse(Configuration["RabbitMQ:Port"]),
                UserName = Configuration["RabbitMQ:UserName"] == null ? "" : Configuration["RabbitMQ:UserName"],
                Password = Configuration["RabbitMQ:Password"] == null ? "" : Configuration["RabbitMQ:Password"]
            };
        }

    }
}
