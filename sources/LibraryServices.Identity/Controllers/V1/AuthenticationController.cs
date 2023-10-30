using Asp.Versioning;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServices.Identity.Controllers.V1
{
    [Route("auth/v1")]
    [ApiVersion("1.0")]
    public class AuthenticationController: ApiControllerBase
    {
        public AuthenticationController()
        {
            
        }
    }
}
