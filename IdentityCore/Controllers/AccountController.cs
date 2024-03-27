using IdentityCore.Identity;
using IdentityCore.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityCoreUser> userManager;
        private readonly SignInManager<IdentityCoreUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AccountController(UserManager<IdentityCoreUser> userManager, 
            SignInManager<IdentityCoreUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is not null)
                return BadRequest("Email registered already");

            var newUser = new IdentityCoreUser()
            {
                UserName = model.UserName,
                PasswordHash = model.Password,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };
            var result = await userManager.CreateAsync(newUser, newUser.PasswordHash!);
            if(!result.Succeeded)
            {
                return BadRequest("Internal Error. Please try again");
            }
            //Assign Role
            var role = await roleManager.FindByNameAsync(model.Role);
            if(role is null && model.Role == Enum.GetName(typeof(Roles), Roles.Admin)!)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = Enum.GetName(typeof(Roles), Roles.Admin) });
            }
            else if(role is null && model.Role == Enum.GetName(typeof(Roles), Roles.User)!)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = Enum.GetName(typeof(Roles), Roles.User) });
            }
            else
            {
                return BadRequest("Role not exist");
            }
            var resultRole = await userManager.AddToRoleAsync(newUser, model.Role);
            if(resultRole.Succeeded)
            {
                return Ok(resultRole);
            }
            return BadRequest("Bad Request");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model is null)
            {
                return BadRequest("Bad Request");
            }

            var user = await userManager.FindByEmailAsync(model.Email!);
            if (user is null) { return BadRequest("User not Found"); }

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName !),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = GetToken(authClaims);

                return Ok(token);
            }
            return BadRequest("Password incorrect");
        }

        [HttpGet("Logout")]
        public void Logout()
        {
            signInManager.SignOutAsync();
        }

        private string GetToken(List<Claim> authClaims)
        {
            var secirutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SigningKey"]!));
            var credentials = new SigningCredentials(secirutyKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"]!,
                audience: config["JWT:ValidAudience"]!,
                claims: authClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
