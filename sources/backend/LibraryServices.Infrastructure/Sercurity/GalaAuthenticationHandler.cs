using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Encodings.Web;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class GalaAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public GalaAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new NotImplementedException();
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            var message = JsonConvert.SerializeObject(new GalaApiResponse(StatusCode.Code401).Message);
            await Response.WriteAsync(message);
        }

        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            var message = JsonConvert.SerializeObject(new GalaApiResponse(StatusCode.Code403).Message);
            await Response.WriteAsync(message);
        }
    }
}
