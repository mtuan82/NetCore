﻿using Microsoft.AspNetCore.Identity;

namespace IdentityCore.Model
{
    public class RegisterModel
    {
        public bool TwoFactorEnabled { get; set; }

        public string? PhoneNumber { get; set; }
   
        public required string Email { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public required string Role { get; set; }
    }
}
