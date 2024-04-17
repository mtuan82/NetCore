using Microsoft.IdentityModel.Tokens;

namespace IdentityCore.Configuration
{
    public static class TokenValidators
    {
        public static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}
