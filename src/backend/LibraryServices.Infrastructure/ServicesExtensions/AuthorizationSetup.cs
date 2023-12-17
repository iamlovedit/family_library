﻿using LibraryServices.Infrastructure.Sercurity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
using System.Security.Claims;
using System.Text;

namespace LibraryServices.Infrastructure.ServicesExtensions
{
    public static class AuthorizationSetup
    {
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);

            var audienceSection = configuration.GetSection("Audience");
            var keyByteArray = Encoding.ASCII.GetBytes(configuration["AUDIENCE_KEY"]!);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var issuer = audienceSection["Issuer"];
            var audience = audienceSection["Audience"];
            var expiration = audienceSection["Expiration"];

            services.AddSingleton(new PermissionRequirement(ClaimTypes.Role, issuer!, audience!,
                TimeSpan.FromSeconds(expiration.ObjToInt()), signingCredentials));
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PermissionConstants.POLICY_NAME,
                    policy => policy.RequireRole(PermissionConstants.CONSUMER, PermissionConstants.ADMINISTATOR, PermissionConstants.SUPERADMINISTATOR).Build());
            });
        }
    }
}
