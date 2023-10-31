using Asp.Versioning;
using AutoMapper;
using LibraryServices.Domain.DataTransferObjects.Identity;
using LibraryServices.Domain.Models.Identity;
using LibraryServices.IdentityService.Services;
using LibraryServices.Infrastructure.RedisCache;
using LibraryServices.Infrastructure.Repository;
using LibraryServices.Infrastructure.Sercurity;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using SqlSugar.Extensions;

namespace LibraryServices.IdentityService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("identity/v{version:apiVersion}/user")]
    public class UserController : ApiControllerBase
    {
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ILogger<UserController> _logger;
        private readonly IRedisBasketRepository _redis;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ITokenBuilder tokenBuilder, ILogger<UserController> logger,
            IRedisBasketRepository redis, IMapper mapper, IUserService userService, IUnitOfWork unitOfWork)
        {
            _tokenBuilder = tokenBuilder;
            _logger = logger;
            _redis = redis;
            _mapper = mapper;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("exist")]
        public async Task<MessageData<bool>> IsExistsAsync(string username)
        {
            var redisKey = RedisKeyHelper.GetUserByUsernameKey(username);
            var user = default(User);
            if (await _redis.Exist(redisKey))
            {
                user = await _redis.Get<User>(redisKey);
                return Success(true);
            }
            else
            {
                user = await _userService.GetFirstByExpressionAsync(u => u.Username == username);
                if (user is null)
                {
                    return Failed<bool>("user not exists");
                }

                await _redis.Set(redisKey, user, TimeSpan.FromDays(7));
                return Success(true);
            }
        }

        [HttpGet("me")]
        public async Task<MessageData<UserDTO>> GetUserDetails()
        {
            var userId = GetUserIdFromClaims();
            if (userId == 0)
            {
                return Failed<UserDTO>("obtain user id failed");
            }

            var redisKey = RedisKeyHelper.GetUserByIdKey(userId);
            var user = default(User);
            if (await _redis.Exist(redisKey))
            {
                user = await _redis.Get<User>(redisKey);
                return Success(_mapper.Map<UserDTO>(user));
            }

            user = await _userService.GetByIdAsync(userId);
            if (user is null)
            {
                return Failed<UserDTO>("user not exists");
            }

            await _redis.Set(redisKey, user, TimeSpan.FromDays(7));
            return Success(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<MessageData<TokenInfo>> RegisterAsync([FromBody] UserCreationDTO userCreationDTO,
            [FromServices] IRoleService roleService, [FromServices] IUserRoleService userRoleService)
        {
            var redisKey = $"user/register/username={userCreationDTO.Username}&password={userCreationDTO.Password}";
            if (await _redis.Exist(redisKey))
            {
                return Failed<TokenInfo>("invalid request", 400);
            }

            await _redis.Set(redisKey, userCreationDTO, TimeSpan.FromSeconds(1));

            var user = await _userService.GetFirstByExpressionAsync(u => u.Username == userCreationDTO.Username);
            if (user != null)
            {
                return Failed<TokenInfo>("user already exists!", 300);
            }

            user = _mapper.Map<User>(userCreationDTO);
            user.Password = userCreationDTO.Password!.MD5Encrypt32(user.Salt!);
            var role = await roleService.GetFirstByExpressionAsync(r => r.Name == PermissionConstants.CONSUMER);
            if (role is null)
            {
                _logger.LogWarning("role: {roleName} not exists", PermissionConstants.CONSUMER);
                return Failed<TokenInfo>("register failed", 500);
            }

            try
            {
                _unitOfWork.BeginTransaction();
                var userId = await _userService.AddSnowflakeAsync(user);
                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = userId
                };
                await userRoleService.AddSnowflakeAsync(userRole);
                await _redis.Set(RedisKeyHelper.GetUserByIdKey(userId), user, TimeSpan.FromDays(7));
                _unitOfWork.CommitTransaction();

                var claims = new List<Claim>()
                {
                    new(ClaimTypes.Name, user.Username!),
                    new(JwtRegisteredClaimNames.Jti, user.Id.ObjToString()),
                    new(ClaimTypes.Expiration,
                        DateTime.Now.AddSeconds(_tokenBuilder.GetTokenExpirationSeconds()).ToString()),
                    new(ClaimTypes.Role, role.Name!)
                };

                var token = _tokenBuilder.GenerateTokenInfo(claims);
                return Success(token);
            }
            catch (Exception e)
            {
                _unitOfWork.RollbackTransaction();
                return Failed<TokenInfo>(e.Message, 500);
            }
        }
    }
}
