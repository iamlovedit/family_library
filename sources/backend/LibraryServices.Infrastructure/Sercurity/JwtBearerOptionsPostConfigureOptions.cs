using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class JwtBearerOptionsPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly SecurityTokenValidator _securityTokenValidator;

        public JwtBearerOptionsPostConfigureOptions(SecurityTokenValidator securityTokenValidator)
        {
            _securityTokenValidator = securityTokenValidator;
        }

        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.SecurityTokenValidators.Clear();
            options.SecurityTokenValidators.Add(_securityTokenValidator);
        }

    }
}
