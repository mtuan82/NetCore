using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace DataCore.Configurations
{
    public class RedisSettings : IValidatable
    {
        public bool Enabled { get; set; } = true;

        [Required]
        public required string Host { get; set; }


        public int Port { get; set; }

        public int PoolSize { get; set; } = 20;

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
