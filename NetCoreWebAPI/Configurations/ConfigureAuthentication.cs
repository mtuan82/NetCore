using Core.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Core.Authorization;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureAuthentication
    {
        public static void AddAuthentication(this IServiceCollection services, IdentitySettings appSettings)
        {
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

            var issuers = GetList(appSettings.Issuers);

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            if (appSettings.IsLocal)
            {
                authenticationBuilder.AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(appSettings.SigningKey);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireSignedTokens = true,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        LifetimeValidator = LifetimeValidator,
                    };
                });
            }
            else
            {
                foreach (var issuer in issuers)
                {
                    authenticationBuilder.AddJwtBearer($"{JwtBearerDefaults.AuthenticationScheme}_{issuer}", async options =>
                    {
                        var cm = new ConfigurationManager<OpenIdConnectConfiguration>(issuer + "/.well-known/openid-configuration", new OpenIdConnectConfigurationRetriever());
                        var openIDConfig = await cm.GetConfigurationAsync();

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                            {
                                return openIDConfig.SigningKeys;
                            },

                            ValidIssuer = issuer,
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateLifetime = true,
                            LifetimeValidator = LifetimeValidator,
                            ValidateAudience = false //aws cognito is not passing audience,
                        };

                        options.Authority = issuer;
                        options.RequireHttpsMetadata = appSettings.ValidateHttps;

                    });
                }
            }

            services.AddAuthorization(options =>
            {
                if (!appSettings.IsLocal)
                {
                    var authSchemes = issuers.Select(i => $"{JwtBearerDefaults.AuthenticationScheme}_{i}").ToArray();
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(authSchemes)
                    .Build();
                }
                else
                {
                    options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                }
                options.AddPolicy("api.create",
                     policy =>
                     {
                         policy.RequireRole("Admin");
                         policy.Requirements.Add(
                                 new HasScopeRequirement(
                                     appSettings.IdentityDomain,
                                     appSettings.IdentityDomain + "api.create",
                                     issuers
                                     )
                                 );
                     });
                options.AddPolicy("api.update",
                     policy =>
                     {
                         policy.RequireRole("Admin");
                         policy.Requirements.Add(
                                 new HasScopeRequirement(
                                     appSettings.IdentityDomain,
                                     appSettings.IdentityDomain + "api.update",
                                     issuers
                                     )
                                 );
                     });
                options.AddPolicy("api.read",
                     policy =>
                     {
                         policy.RequireRole("Admin","User");
                         policy.Requirements.Add(
                           new HasScopeRequirement(
                               appSettings.IdentityDomain,
                               appSettings.IdentityDomain + "api.read",
                               issuers
                               )
                           );
                     });
                options.AddPolicy("api.delete",
                     policy =>
                     {
                         policy.RequireRole("Admin");
                         policy.Requirements.Add(
                            new HasScopeRequirement(
                                appSettings.IdentityDomain,
                                appSettings.IdentityDomain + "api.delete",
                                issuers
                                )
                            );
                     });
            });
        }

        private static List<string> GetList(string settings)
        {
            return new List<string>(settings.Split(","));
        }

        private static bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
}
