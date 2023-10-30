using LibraryServices.Infrastructure.Sercurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class JwtAuthenticationSetup
    {
        public static void AddJwtAuthenticationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection("Audience");
            var buffer = Encoding.UTF8.GetBytes(configuration["AUDIENCE_KEY"]!);
            var key = new SymmetricSecurityKey(buffer);
            var issuer = section["Issuer"];
            var audience = section["Audience"];
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(15),
                RequireExpirationTime = true,
                RoleClaimType = ClaimTypes.Role
            };

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ApiAuthenticationHandler);
                options.DefaultForbidScheme = nameof(ApiAuthenticationHandler);
            }).AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>(nameof(ApiAuthenticationHandler),
                options => { }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = challengeContext =>
                        {
                            challengeContext.Response.Headers.Add("Token-Error", challengeContext.ErrorDescription);
                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = failedContext =>
                        {
                            var jwtHandler = new JwtSecurityTokenHandler();
                            var token = failedContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                            if (string.IsNullOrEmpty(token) || !jwtHandler.CanReadToken(token))
                            {
                                failedContext.Response.Headers.Add("token-error", "can not get token");
                                return Task.CompletedTask;
                            }

                            if (failedContext.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                failedContext.Response.Headers.Add("token-expired", "true");
                                return Task.CompletedTask;
                            }

                            if (jwtHandler.CanReadToken(token))
                            {
                                try
                                {
                                    var jwtToken = jwtHandler.ReadJwtToken(token);
                                    if (jwtToken.Issuer != issuer)
                                    {
                                        failedContext.Response.Headers.Add("token-error-issuer", "issuer is wrong!");
                                        return Task.CompletedTask;
                                    }

                                    if (jwtToken.Audiences.FirstOrDefault() != audience)
                                    {
                                        failedContext.Response.Headers.Add("token-error-audience", "audience is wrong!");
                                        return Task.CompletedTask;
                                    }
                                }
                                catch (Exception)
                                {
                                    failedContext.Response.Headers.Add("token-error-format", "token format is wrong!");
                                    return Task.CompletedTask;
                                }
                            }
                            else
                            {
                                failedContext.Response.Headers.Add("token-error-format", "token format is wrong!");
                                return Task.CompletedTask;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
