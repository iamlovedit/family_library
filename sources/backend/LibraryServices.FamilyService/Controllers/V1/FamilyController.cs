using Asp.Versioning;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Minio;

namespace LibraryServices.FamilyService.Controllers.V1
{
    [Route("family/v1")]
    [ApiVersion("1.0")]
    public class FamilyController:ApiControllerBase
    {
        private readonly IMinioClient _minioClient;
        public FamilyController(IMinioClient minioClient)
        {
            _minioClient = minioClient;
        }
    }
}
