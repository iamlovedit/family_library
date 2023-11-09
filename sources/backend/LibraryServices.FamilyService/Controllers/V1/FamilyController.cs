using Asp.Versioning;
using AutoMapper;
using LibraryServices.Domain.DataTransferObjects.FamilyLibrary;
using LibraryServices.Domain.Models.FamilyLibrary;
using LibraryServices.FamilyService.Services;
using LibraryServices.Infrastructure;
using LibraryServices.Infrastructure.RedisCache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using SqlSugar;

namespace LibraryServices.FamilyService.Controllers.V1
{
    [Route("family/{version:apiVersion}")]
    [ApiVersion("1.0")]
    public class FamilyController : ApiControllerBase
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<FamilyController> _logger;
        private readonly IRedisBasketRepository _redis;
        private readonly IMapper _mapper;
        private readonly IFamilyService _familyService;
        private readonly RedisRequirement _redisRequirement;
        private static readonly string _bucketName = "family-bucket";
        private static string _region = "ShangHai";
        private static readonly int _expiry = 60;

        public FamilyController(IMinioClient minioClient, ILogger<FamilyController> logger,
            IRedisBasketRepository redis,
            IMapper mapper, IFamilyService familyService, RedisRequirement redisRequirement)
        {
            _minioClient = minioClient;
            _logger = logger;
            _redis = redis;
            _mapper = mapper;
            _familyService = familyService;
            _redisRequirement = redisRequirement;
        }

        [HttpGet]
        [Route("{id:long}/{familyVersion:int}")]
        public async Task<IActionResult> DownloadFamilyAsync(long id, ushort familyVersion)
        {
            var redisKey = RedisKeyHelper.GetFamilyByIdKey(id);
            var family = default(Family);
            if (await _redis.Exist(redisKey))
            {
                family = await _redis.Get<Family>(redisKey);
            }
            else
            {
                family = await _familyService.GetByIdAsync(id);
            }

            if (family is null)
            {
                return Ok("family not exist");
            }

            var file = family.GetFilePath(familyVersion);
            var fileUrl = await _minioClient.PresignedGetObjectAsync(new PresignedGetObjectArgs()
                .WithBucket(_bucketName).WithObject(file).WithExpiry(_expiry));
            return Redirect(fileUrl);
        }

        [HttpGet]
        [Route("details/{id:long}")]
        public async Task<MessageData<FamilyDetailDTO>> GetFamilyDetailAsync(long id)
        {
            var redisKey = $"familyDetails/{id}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<FamilyDetailDTO>(redisKey));
            }

            var family = await _familyService.GetFamilyDetails(id);
            if (family is null)
            {
                _logger.LogWarning("query family details failed id: {id} ,family not existed", id);
                return Failed<FamilyDetailDTO>("family not exist", 404);
            }

            _logger.LogInformation("query family details succeed id: {id}", id);
            var familyDto = _mapper.Map<FamilyDetailDTO>(family);
            await _redis.Set(redisKey, familyDto, _redisRequirement.CacheTime);
            return Success(familyDto);
        }

        [HttpGet]
        [Route("categories")]
        [AllowAnonymous]
        public async Task<MessageData<List<FamilyCategoryDTO>>> GetCategoriesAsync([FromQuery] int? parentId = null)
        {
            _logger.LogInformation("query child categories by parent {parentId}", parentId);
            var redisKey = parentId == null ? $"family/categories" : $"family/categories?parentId={parentId}";
            if (await _redis.Exist(redisKey))
            {
                return Success(await _redis.Get<List<FamilyCategoryDTO>>(redisKey));
            }

            var categories = await _familyService.GetCategoryTreeAsync(parentId);
            var categoriesDto = _mapper.Map<List<FamilyCategoryDTO>>(categories);
            await _redis.Set(redisKey, categoriesDto, TimeSpan.FromDays(1));
            return Success(categoriesDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<MessageData<PageData<FamilyBasicDTO>>> GetFamiliesPageAsync(string? keyword = null,
            long? categoryId = null, int pageIndex = 1, int pageSize = 30, string? order = "name")
        {
            var redisKey =
                $"families?keyword={keyword ?? "null"}&categoryId={categoryId}&pageIndex={pageIndex}" +
                $"&pageSize={pageSize}&orderField={order}";
            if (await _redis.Exist(redisKey))
            {
                return SucceedPage(await _redis.Get<PageData<FamilyBasicDTO>>(redisKey));
            }

            _logger.LogInformation(
                "query families by category {category} and keyword {keyword} at page {page} pageSize {pageSize}",
                categoryId, keyword, pageIndex, pageSize);
            var expression = Expressionable.Create<Family>()
                .AndIF(categoryId != null, f => f.CategoryId == categoryId)
                .AndIF(keyword != null, f => f.Name!.Contains(keyword))
                .ToExpression();

            var familyPage = await _familyService.GetFamilyPageAsync(expression, pageIndex,
                pageSize, $"{order} DESC");
            var familyPageDto = familyPage.ConvertTo<FamilyBasicDTO>(_mapper);
            await _redis.Set(redisKey, familyPageDto, _redisRequirement.CacheTime);
            return SucceedPage(familyPageDto);
        }
    }
}