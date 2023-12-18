using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly IAESEncryptionService _aesEncryptionService;
        private readonly PermissionRequirement _permissionRequirement;
        private readonly IConfiguration _configuration;

        public TokenBuilder(IAESEncryptionService aesEncryptionService, PermissionRequirement permissionRequirement, IConfiguration configuration)
        {
            _aesEncryptionService = aesEncryptionService;
            _permissionRequirement = permissionRequirement;
            _configuration = configuration;
        }

        public string DecryptCipherToken(string cipherToken)
        {
            if (string.IsNullOrEmpty(cipherToken))
            {
                throw new ArgumentException($"{nameof(cipherToken)} is null or empty。", nameof(cipherToken));
            }

            return _aesEncryptionService.Decrypt(cipherToken);
        }

        public TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims)
        {
            var jwtToken = new JwtSecurityToken(
                   issuer: _permissionRequirement.Issuer,
                   audience: _permissionRequirement.Audience,
                   claims: claims,
                   notBefore: DateTime.Now,
                   expires: DateTime.Now.Add(_permissionRequirement.Expiration),
                   signingCredentials: _permissionRequirement.SigningCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            token = _aesEncryptionService.Encrypt(token);
            return new TokenInfo(token, _permissionRequirement.Expiration.TotalSeconds, JwtBearerDefaults.AuthenticationScheme);
        }

        public double GetTokenExpirationSeconds()
        {
            return _permissionRequirement.Expiration.TotalSeconds;
        }

        public long ParseUIdFromToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (jwtHandler.CanReadToken(token))
            {
                var jwtToken = jwtHandler.ReadJwtToken(token);
                if (long.TryParse(jwtToken.Id, out var id))
                {
                    return id;
                }
            }
            return 0;
        }

        public bool VerifyToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var key = _configuration["AUDIENCE_KEY"];
            var keyBuffer = Encoding.ASCII.GetBytes(key!);
            var signingKey = new SymmetricSecurityKey(keyBuffer);
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = jwtHandler.ReadJwtToken(token);
            return jwt.RawSignature == JwtTokenUtilities.CreateEncodedSignature(jwt.RawHeader + "." + jwt.RawPayload, signingCredentials);
        }
    }
}
