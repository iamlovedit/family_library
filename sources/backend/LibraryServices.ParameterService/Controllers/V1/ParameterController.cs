using Asp.Versioning;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServices.ParameterService.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("parameter/v{version:apiVersion}")]
    public class ParameterController : ApiControllerBase
    {
        public ParameterController()
        {

        }
    }
}
