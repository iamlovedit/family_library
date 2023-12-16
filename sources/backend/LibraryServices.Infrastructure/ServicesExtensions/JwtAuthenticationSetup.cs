using LibraryServices.Infrastructure.Sercurity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
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
                             challengeContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error", challengeContext.ErrorDescription));
                             return Task.CompletedTask;
                         },
                     };
                 });
        }
    }
}
