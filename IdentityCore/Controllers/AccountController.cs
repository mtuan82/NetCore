using Core.Enum;
using Core.Providers.PostgreSQL.Entity;
using IdentityCore.Identity;
using IdentityCore.Model;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Register([FromBody]RegisterModel model)
        {
            try
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                    return BadRequest(new ResultModel() { Error = "Email registered already" });

                var newUser = new IdentityCoreUser()
                {
                    UserName = model.UserName,
                    PasswordHash = model.Password,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    TwoFactorEnabled = model.TwoFactorEnabled
                };
                var result = await userManager.CreateAsync(newUser, newUser.PasswordHash!);
                if (!result.Succeeded)
                {
                    return BadRequest(new ResultModel() { Error = "Internal Error. Please try again" });
                }
                //Assign Role
                var role = await roleManager.FindByNameAsync(model.Role);
                if (role is null && model.Role == Enum.GetName(typeof(Roles), Roles.Admin)!)
                {
                    var idRole = new IdentityRole() { Name = Enum.GetName(typeof(Roles), Roles.Admin) };
                    await roleManager.CreateAsync(idRole);
                    await roleManager.AddClaimAsync(idRole, new Claim("scope", config["JWT:IdentityDomain"] + "api.create"));
                    await roleManager.AddClaimAsync(idRole, new Claim("scope", config["JWT:IdentityDomain"] + "api.update"));
                    await roleManager.AddClaimAsync(idRole, new Claim("scope", config["JWT:IdentityDomain"] + "api.read"));
                    await roleManager.AddClaimAsync(idRole, new Claim("scope", config["JWT:IdentityDomain"] + "api.delete"));
                }
                else if (role is null && model.Role == Enum.GetName(typeof(Roles), Roles.User)!)
                {
                    var idRole = new IdentityRole() { Name = Enum.GetName(typeof(Roles), Roles.User) };
                    await roleManager.CreateAsync(idRole);
                    await roleManager.AddClaimAsync(idRole, new Claim("scope", config["JWT:IdentityDomain"] + "/api.read"));
                }
                else if (role is null)
                {
                    return BadRequest(new ResultModel() { Error = "Role not exist" });
                }
                var resultRole = await userManager.AddToRoleAsync(newUser, model.Role);
                if (resultRole.Succeeded)
                {
                    return Ok(new ResultModel() { IsSuccessful = true });
                }
                return BadRequest(new ResultModel() { Error = "Bad Request" });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultModel() { Error = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            if (model is null)
            {
                return BadRequest(new ResultModel() { Error = "Bad Request" });
            }

            var user = await userManager.FindByEmailAsync(model.Email!);
            if (user is null) { return BadRequest(new ResultModel() { Error = "User not Found" }); }

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
                    var role = await roleManager.FindByNameAsync(userRole);
                    if (role != null)
                    {
                        var claims = await roleManager.GetClaimsAsync(role);
                        foreach (var claim in claims)
                        {
                            authClaims.Add(new Claim("scope", claim.Value));
                        }
                    }
                }
                var token = GetToken(authClaims);

                return Ok(new ResultModel() { Token = token, IsSuccessful = true });
            }
            return BadRequest(new ResultModel() { Error = "Password incorrect" });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new ResultModel() { IsSuccessful = true });
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

        [Authorize(Roles = "User")]
        [HttpGet("testexpiration")]
        public async Task<IActionResult> testexpiration()
        {
            return Ok(new ResultModel() { IsSuccessful = true });
        }
    }
}
