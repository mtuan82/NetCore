using Core.Enum;
using IdentityCore.Identity;
using IdentityCore.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace IdentityCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Account2FAController : ControllerBase
    {
        private readonly UserManager<IdentityCoreUser> userManager;
        private readonly SignInManager<IdentityCoreUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public Account2FAController(UserManager<IdentityCoreUser> userManager,
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
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
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
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model is null)
            {
                return BadRequest(new ResultModel() { Error = "Bad Request" });
            }

            var user = await userManager.FindByEmailAsync(model.Email!);
            if (user is null) 
            { 
                return BadRequest(new ResultModel() { Error = "User not Found" }); 
            }

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password!))
            {
                var authClaims = await GetClaim(user);
                if(authClaims.IsNullOrEmpty())
                    return BadRequest(new ResultModel() { Error = "Role invalid" });
                var code = await userManager.GenerateTwoFactorTokenAsync(user,TokenOptions.DefaultEmailProvider);
                SendEmail2FACode(code, user);
                return Ok(new ResultModel() { Code = code, IsSuccessful = true });
            }
            return BadRequest(new ResultModel() { Error = "Password incorrect" });
        }

        [HttpPost("Verify2FA")]
        public async Task<IActionResult> Verify2FA(string email, string code)
        {
            var user = await userManager.FindByEmailAsync(email);
            bool isValid = await userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, code);
            if(isValid)
            {
                var authClaims = await GetClaim(user);
                return Ok(new ResultModel() { Token = GetToken(authClaims), IsSuccessful = true });
            }
            return BadRequest(new ResultModel() { Error = "Code invalid", IsSuccessful = false });
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new ResultModel() { IsSuccessful = true });
        }

        private bool SendEmail2FACode(string code,IdentityCoreUser user)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("NoReply@gmail.com");
            mailMessage.To.Add(new MailAddress(user.Email));
            mailMessage.Subject = "Your one-time security code";
            StringBuilder body = new StringBuilder();
            body.Append("Hello, " + user.FirstName);
            body.Append("We want to help you log in. To keep your account safe, we created a one-time security code for you.");
            body.Append("Your security code is " + code);
            body.Append("The code is temporary and will expire in 30 minutes. Please enter it into the form on the website to confirm your online access.");
            mailMessage.Body = code;

            SmtpClient sMTPClient = new SmtpClient();
            sMTPClient.EnableSsl = true;
            sMTPClient.Host = "smtp.gmail.com";
            sMTPClient.Port = 587;
            sMTPClient.UseDefaultCredentials = false;
            sMTPClient.Credentials = new System.Net.NetworkCredential("sender's email Address", "password");
            sMTPClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            sMTPClient.Send(mailMessage);
            return true;
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

        private async Task<List<Claim>> GetClaim(IdentityCoreUser user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName !),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };
            var userRoles = await userManager.GetRolesAsync(user);
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
            return authClaims;
        }
    }
}
