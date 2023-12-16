using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class GalaTokenHandler : TokenHandler
    {
        private readonly IAESEncryptionService _aesEncryptionService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        public GalaTokenHandler(IAESEncryptionService aesEncryptionService, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _aesEncryptionService = aesEncryptionService;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }
        public override Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
        {
            var decodeToken = _aesEncryptionService.Decrypt(token);
            return _jwtSecurityTokenHandler.ValidateTokenAsync(decodeToken, validationParameters);
        }
    }
}
