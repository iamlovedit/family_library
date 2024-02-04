using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryServices.Infrastructure.Lock;

public class RequestLockMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLockMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        if (endpoint != null)
        {
            var requestLock = endpoint.Metadata.GetMetadata<RequestLockAttribute>();
            if (requestLock is null)
            {
                await _next(context);
            }
            else
            {
                //context.RequestServices.GetRequiredService<IRedisLock>();
            }
        }
    }
}