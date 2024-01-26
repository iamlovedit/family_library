using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar.Extensions;

namespace LibraryServices.Infrastructure.Email
{
    public static class EmailSetup
    {
        public static void AddEmailSetup(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);

            services.AddSingleton(provider =>
            {
                var option = new SmtpOption
                {
                    Server = configuration["SMTP_SERVER"],
                    Port = configuration["SMTP_PORT"].ObjToInt(),
                    Sender = configuration["SMTP_SENDER"],
                    SenderAddress = configuration["SMTP_SENDER_ADDRESS"],
                    Username = configuration["SMTP_USERNAME"],
                    Password = configuration["SMTP_PASSWORD"],
                    UseSSL = configuration["SMTP_USESSL"].ObjToBool()
                };
                return option;
            });
            services.AddScoped<IEmailSender, EmailSender>();
        }
    }
}
