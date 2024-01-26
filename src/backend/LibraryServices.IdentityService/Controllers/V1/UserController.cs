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
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using FluentValidation;
using LibraryServices.Infrastructure.Email;

namespace LibraryServices.IdentityService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("identity/v{version:apiVersion}/user")]
    public class UserController : GalaApiControllerBase
    {
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ILogger<UserController> _logger;
        private readonly IRedisBasketRepository _redis;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UserCreationDTO> _validator;

        public UserController(ITokenBuilder tokenBuilder, ILogger<UserController> logger,
            IValidator<UserCreationDTO> validator,
            IRedisBasketRepository redis, IMapper mapper, IUserService userService, IUnitOfWork unitOfWork)
        {
            _tokenBuilder = tokenBuilder;
            _logger = logger;
            _redis = redis;
            _mapper = mapper;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _validator = validator;
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

            await _redis.Set(redisKey, redisKey, TimeSpan.FromSeconds(1));

            var validResult = await _validator.ValidateAsync(userCreationDTO);
            if (!validResult.IsValid)
            {
                return Failed<TokenInfo>(string.Join(',', validResult.Errors.Select(e => e.ErrorMessage)));
            }

            var cacheKey =
                $"user/email?type=cache&username={userCreationDTO.Username}&password={userCreationDTO.Password}&email={userCreationDTO.Email}";
            if (!await _redis.Exist(cacheKey))
            {
                return Failed<TokenInfo>("valid code not found!", 410);
            }

            var emailCode = await _redis.Get<int>(cacheKey);
            if (userCreationDTO.VaildCode != emailCode)
            {
                return Failed<TokenInfo>("valid code is not matched!", 411);
            }

            await _redis.Remove(cacheKey);

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
                    new(JwtRegisteredClaimNames.Iat,
                        EpochTime.GetIntDate(DateTime.Now).ToString(CultureInfo.InvariantCulture),
                        ClaimValueTypes.Integer64),
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


        [HttpPost("email")]
        [AllowAnonymous]
        public async Task<MessageData<bool>> SendEmailAsync([FromBody] UserCreationDTO userCreationDTO,
            [FromServices] IEmailSender emailSender)
        {
            var lockKey =
                $"user/email?type=request&username={userCreationDTO.Username}&password={userCreationDTO.Password}&email={userCreationDTO.Email}";
            if (await _redis.Exist(lockKey))
            {
                return Failed<bool>("invalid request");
            }

            await _redis.Set(lockKey, userCreationDTO, TimeSpan.FromSeconds(5));

            var validateResult = await _validator.ValidateAsync(userCreationDTO);
            if (!validateResult.IsValid)
            {
                var message = string.Join(',', validateResult.Errors.Select(e => e.ErrorMessage).ToArray());
                return Failed<bool>(message);
            }

            var cacheKey =
                $"user/email?type=cache&username={userCreationDTO.Username}&password={userCreationDTO.Password}&email={userCreationDTO.Email}";

            if (await _redis.Exist(cacheKey))
            {
                return Failed<bool>(code: 400);
            }

            var code = Random.Shared.Next(100001, 999999);
            var content = $"""
                           您好：
                               欢迎注册我的网站，为了安全请将以下验证码输入到注册页面,5分钟内有效。
                           
                               {code}
                           
                               Youngala
                           """;
            var emailMessage = new EmailMessage(userCreationDTO.Username, userCreationDTO.Email, "注册验证", content);
            var result = await emailSender.SendTextEmailAsync(emailMessage);
            if (!result)
            {
                return Failed<bool>();
            }

            await _redis.Set(cacheKey, code, TimeSpan.FromMinutes(5));
            return Success(true);
        }
    }
}