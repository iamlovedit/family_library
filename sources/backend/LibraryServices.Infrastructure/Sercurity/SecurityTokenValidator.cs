using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class SecurityTokenValidator : ISecurityTokenValidator
    {
        private readonly IAESEncryptionService _aesEncryptionService;

        public SecurityTokenValidator(IAESEncryptionService aesEncryptionService)
        {
            _aesEncryptionService = aesEncryptionService;
        }

        public bool CanValidateToken { get; } = true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken)
        {
            return CanValidateToken;
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            var decodeToken = _aesEncryptionService.Decrypt(securityToken);
            return new JwtSecurityTokenHandler().ValidateToken(decodeToken, validationParameters, out validatedToken);
        }
    }
}
