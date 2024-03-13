using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace DataCore.Configurations
{
    public class MySQLSettings : IValidatable
    {
        public required string Host { get; set; }

        public required string Port { get; set; }

        public required string Database { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public string ConnectionString =>
            $"server={Host};Port={Port};database={Database};user={UserName};password='{Password}';";

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }

    }
}
