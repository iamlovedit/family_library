using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using SqlSugar.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace LibraryServices.Infrastructure.Sercurity
{
    public class JwtBearerOptionsPostConfigureOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly GalaTokenHandler _galaTokenHandler;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtBearerOptionsPostConfigureOptions(GalaTokenHandler galaTokenHandler, JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _galaTokenHandler = galaTokenHandler;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public void PostConfigure(string? name, JwtBearerOptions options)
        {
            options.TokenHandlers.Clear();
            options.TokenHandlers.Add(_galaTokenHandler);
            options.Events.OnAuthenticationFailed = failedContext =>
            {

                var token = failedContext.Request.Headers["Authorization"].ObjToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token) || !_jwtSecurityTokenHandler.CanReadToken(token))
                {
                    failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error", "can't get token"));
                    return Task.CompletedTask;
                }

                if (failedContext.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-expired", "true"));
                    return Task.CompletedTask;
                }

                if (_jwtSecurityTokenHandler.CanReadToken(token))
                {
                    try
                    {
                        var jwtToken = _jwtSecurityTokenHandler.ReadJwtToken(token);
                        if (jwtToken.Issuer != options.TokenValidationParameters.ValidIssuer)
                        {
                            failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error-issuer", "issuer is wrong"));
                            return Task.CompletedTask;
                        }

                        if (jwtToken.Audiences.FirstOrDefault() != options.TokenValidationParameters.ValidIssuer)
                        {
                            failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error-audience", "audience is wrong!"));
                            return Task.CompletedTask;
                        }
                    }
                    catch (Exception)
                    {
                        failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error-format", "token format is wrong!"));
                        return Task.CompletedTask;
                    }
                }
                else
                {
                    failedContext.Response.Headers.Append(new KeyValuePair<string, StringValues>("token-error-format", "token format is wrong!"));
                    return Task.CompletedTask;
                }

                return Task.CompletedTask;
            };
        }
    }
}
