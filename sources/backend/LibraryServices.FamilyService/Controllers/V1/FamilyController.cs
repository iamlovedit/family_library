using Asp.Versioning;
using LibraryServices.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace LibraryServices.Family.Controllers.V1
{
    [Route("family/v1")]
    [ApiVersion("1.0")]
    public class FamilyController:ApiControllerBase
    {
        public FamilyController()
        {
            
        }
    }
}
