using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace LibraryServices.Infrastructure.Filters
{
    public class GlobalExceptionsFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            return Task.Run(() =>
            {
                if (!context.ExceptionHandled)
                {
                    var message = new MessageData<Exception>(false, context.Exception.Message, 500);
                    context.Result = new ContentResult
                    {
                        StatusCode = StatusCodes.Status200OK,
                        ContentType = "application/json;charset=utf-8",
                        Content = JsonConvert.SerializeObject(message)
                    };
                }
                context.ExceptionHandled = true;
            });
        }
    }
}
