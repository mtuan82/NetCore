using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace Core.Configurations
{
    public class PostgreSQLSettings : IValidatable
    {
        public required string Host { get; set; }

        public required string Port { get; set; }

        public required string Database { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public string ConnectionString =>
            $"Host={Host};Port={Port};Database={Database};Username={UserName};Password='{Password}';";

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
