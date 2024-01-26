using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace LibraryServices.Infrastructure.Email
{
    public interface IEmailSender
    {
        Task<bool> SendTextEmailAsync(EmailMessage emailMessage);
    }

    public class EmailSender : IEmailSender
    {
        private readonly SmtpOption _smtpOption;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(SmtpOption smtpOption, ILogger<EmailSender> logger)
        {
            _smtpOption = smtpOption;
            _logger = logger;
        }
        public async Task<bool> SendTextEmailAsync(EmailMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpOption.Server, _smtpOption.Port, _smtpOption.UseSSL);
                await client.AuthenticateAsync(_smtpOption.Username, _smtpOption.Password);
                await client.SendAsync(CreateTextMessage(emailMessage));
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return false;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        private MimeMessage CreateTextMessage(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpOption.Sender, _smtpOption.SenderAddress));
            message.To.Add(new MailboxAddress(emailMessage.Receiver, emailMessage.RecevieAddress));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart("plain")
            {
                Text = emailMessage.Content
            };
            return message;
        }
    }
}
