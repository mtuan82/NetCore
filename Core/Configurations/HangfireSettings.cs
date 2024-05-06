using NetEscapades.Configuration.Validation;
using System.ComponentModel.DataAnnotations;

namespace Core.Configurations
{
    public  class HangfireSettings : IValidatable
    {
        public required string Cron { get; set; }


        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}
