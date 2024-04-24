using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Configurations
{
    public class IdentitySettings : IValidatable
    {
        public required bool IsLocal { get; set; }

        public required string SigningKey { get; set; }

        public required string Issuers { get; set; }

        public required string IdentityDomain { get; set; }

        public required bool ValidateHttps { get; set; }


        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
