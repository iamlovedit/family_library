using Asp.Versioning;
using LibraryServices.Domain.DataTransferObjects.Identity;
using LibraryServices.IdentityService.Services;
using LibraryServices.Infrastructure;
using LibraryServices.Infrastructure.RedisCache;
using LibraryServices.Infrastructure.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using SqlSugar.Extensions;
using System.Security.Claims;

namespace LibraryServices.Identity.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("identity/v{version:apiVersion}/auth")]
    public class AuthenticationController : ApiControllerBase
    {
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IRedisBasketRepository _redis;
        public AuthenticationController(IRedisBasketRepository redis, ITokenBuilder tokenBuilder, IUserService userService, ILogger<AuthenticationController> logger)
        {
            _redis = redis;
            _tokenBuilder = tokenBuilder;
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<MessageData<TokenInfo>> LoginAsync([FromBody] UserLoginDTO loginUser)
        {
            if (string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
            {
                return Failed<TokenInfo>("password or username is empty");
            }
            var lockKey = $"auth/username={loginUser.Username}&password={loginUser.Password}";
            if (await _redis.Exist(lockKey))
            {
                return Failed<TokenInfo>("invalid request", 400);
            }

            await _redis.Set(lockKey, loginUser, TimeSpan.FromSeconds(1));
            var user = await _userService.GetFirstByExpressionAsync(u => u.Username == loginUser.Username);
            if (user is null)
            {
                return Failed<TokenInfo>("username or password is incorrect");
            }
            var password = loginUser.Password!.MD5Encrypt32(user.Salt!);
            if (password != user.Password)
            {
                return Failed<TokenInfo>("username or password is incorrect");
            }

            var roles = await _userService.GetUserRolesAsync(user.Id);

            var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, user.Username!),
                    new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                    new(ClaimTypes.Expiration,
                        DateTime.Now.AddSeconds(_tokenBuilder.GetTokenExpirationSeconds()).ToString())
                };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name!)));
            var token = _tokenBuilder.GenerateTokenInfo(claims);
            return Success(token);
        }

        [HttpPost("refresh")]
        public async Task<MessageData<TokenInfo>> RefreshTokenAsync([FromForm] string token)
        {
            var lockKey = $"auth/token/refresh/token={token}";
            if (await _redis.Exist(lockKey))
            {
                return Failed<TokenInfo>("invalid request");
            }
            await _redis.Set(lockKey, token, TimeSpan.FromSeconds(1));

            if (string.IsNullOrEmpty(token))
            {
                return Failed<TokenInfo>("token is invalid");
            }
            token = _tokenBuilder.DecryptCipherToken(token);
            var uid = _tokenBuilder.ParseUIdFromToken(token);
            if (_tokenBuilder.VerifyToken(token) && uid > 0)
            {
                var user = await _userService.GetByIdAsync(uid);
                if (user is null)
                {
                    return Failed<TokenInfo>("refresh failed");
                }
                var roles = await _userService.GetUserRolesAsync(uid);
                var claims = new List<Claim>()
                    {
                        new(ClaimTypes.Name, user.Username!),
                        new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                        new(ClaimTypes.Expiration,
                            DateTime.Now.AddSeconds(_tokenBuilder.GetTokenExpirationSeconds()).ToString())
                    };
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name!)));
                var tokenInfo = _tokenBuilder.GenerateTokenInfo(claims);
                return Success(tokenInfo);
            }
            return Failed<TokenInfo>("refresh failed");
        }
    }
}
