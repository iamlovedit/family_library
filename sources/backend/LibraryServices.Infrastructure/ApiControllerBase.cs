using LibraryServices.Infrastructure.Sercurity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace LibraryServices.Infrastructure
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Policy = PermissionConstants.POLICY_NAME)]
    public class ApiControllerBase : ControllerBase
    {
        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageData<T> Success<T>(T data, string message = "成功")
        {
            return new MessageData<T>(true, message, data);
        }


        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageData<T> Failed<T>(string message = "失败", int code = 500)
        {
            return new MessageData<T>(false, message) { StatusCode = code };
        }


        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageData<string> Failed(string message = "失败", int code = 500)
        {
            return new MessageData<string>(false, message) { StatusCode = code };
        }


        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageData<PageData<T>> SucceedPage<T>(int page, int dataCount, int pageSize, List<T> data, int pageCount,
            string message = "获取成功")
        {
            var pageModel = new PageData<T>()
            {
                Data = data,
                PageCount = pageCount,
                PageSize = pageSize,
                Page = page,
                DataCount = dataCount,
            };
            return new MessageData<PageData<T>>(true, message, pageModel);
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public MessageData<PageData<T>> SucceedPage<T>(PageData<T> page, string message = "获取成功")
        {
            var response = new PageData<T>()
            {
                Page = page.Page,
                DataCount = page.DataCount,
                Data = page.Data,
                PageSize = page.PageSize,
                PageCount = page.PageCount,
            };
            return new MessageData<PageData<T>>(true, message, response);
        }

        [NonAction]
        [ApiExplorerSettings(IgnoreApi = true)]
        public long GetUserIdFromClaims()
        {
            return long.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Jti) ?? "0");
        }
    }
}
