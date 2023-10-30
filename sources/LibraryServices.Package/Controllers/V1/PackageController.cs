using Asp.Versioning;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServices.Package.Controllers.V1
{
    [Route("packages/v1")]
    [ApiVersion("1.0")]
    public class PackageController : ApiControllerBase
    {
        public PackageController()
        {

        }
    }
}
