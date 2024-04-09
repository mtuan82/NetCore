using Microsoft.AspNetCore.Identity;

namespace IdentityCore.Identity
{
    public class IdentityCoreUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
