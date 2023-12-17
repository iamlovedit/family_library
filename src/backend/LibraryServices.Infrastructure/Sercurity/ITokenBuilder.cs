using System.Security.Claims;

namespace LibraryServices.Infrastructure.Sercurity
{
    public interface ITokenBuilder
    {
        TokenInfo GenerateTokenInfo(IReadOnlyCollection<Claim> claims);

        string DecryptCipherToken(string cipherToken);

        bool VerifyToken(string token);

        double GetTokenExpirationSeconds();

        long ParseUIdFromToken(string token);
    }
}
