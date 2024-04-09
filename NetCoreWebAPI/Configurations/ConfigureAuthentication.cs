using Core.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NetCoreWebAPI.Configurations
{
    public static class ConfigureAuthentication
    {
        public static void AddAuthentication(this IServiceCollection services, IdentitySettings appSettings)
        {
            var issuers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(appSettings.Issuers);

            var authenticationBuilder = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

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
                        ValidateIssuer = false
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
                            ValidateAudience = false //aws cognito is not passing audience,
                        };

                        options.Authority = issuer;
                        options.RequireHttpsMetadata = appSettings.ValidateHttps;

                    });
                }
            }

            services.AddAuthorization(options =>
            {

                var authSchemes = issuers.Select(i => $"{JwtBearerDefaults.AuthenticationScheme}_{i}").ToArray();

                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(authSchemes)
                    .Build();

                //HasScopeAllHandler for 'all' will grant full access

                //options.AddPolicy("assignment.create",
                //     policy =>
                //         policy.Requirements.Add(
                //             new HasScopeRequirement(
                //                 appSettings.Identity.ScopeBaseDomain,
                //                 appSettings.Identity.ScopeBaseDomain + "/assignment.create",
                //                 issuers
                //                 )
                //             )
                //         );

                //options.AddPolicy("assignment.read",
                //    policy =>
                //        policy.Requirements.Add(
                //            new HasScopeRequirement(
                //                appSettings.Identity.ScopeBaseDomain,
                //                appSettings.Identity.ScopeBaseDomain + "/assignment.read",
                //                issuers
                //                )
                //            )
                //        );

                //options.AddPolicy("configuration.read",
                //    policy =>
                //        policy.Requirements.Add(
                //            new HasScopeRequirement(
                //                appSettings.Identity.ScopeBaseDomain,
                //                appSettings.Identity.ScopeBaseDomain + "/configuration.read",
                //                issuers
                //                )
                //            )
                //        );

                //options.AddPolicy("configuration.write",
                //    policy =>
                //        policy.Requirements.Add(
                //            new HasScopeRequirement(
                //                appSettings.Identity.ScopeBaseDomain,
                //                appSettings.Identity.ScopeBaseDomain + "/configuration.write",
                //                issuers
                //                )
                //            )
                //        );
            });
        }
    }
}
