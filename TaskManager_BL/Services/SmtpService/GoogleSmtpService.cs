using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TaskTracker_BL.Options;

namespace TaskTracker_BL.Services.SmtpService
{
    public class GoogleSmtpService : ISmtpService
    {
        private readonly SmtpOptions _smtpOptions;

        public GoogleSmtpService(IOptions<SmtpOptions> smtpOptions)
        {
            _smtpOptions = smtpOptions.Value;
        }
        public async Task SendEmailAsync(string email, string subject, string messageText)
        {
            var SmtpServer = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = _smtpOptions.Host,
                Port = _smtpOptions.Port,
                Credentials = new NetworkCredential(
                    _smtpOptions.Email,
                    _smtpOptions.Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            var fromMessage = new MailAddress(_smtpOptions.Email);
            var toMessage = new MailAddress(email);
            var message = new MailMessage
            {
                From = fromMessage,
                Subject = subject,
                Body = messageText
            };

            message.To.Add(toMessage);

            await SmtpServer.SendMailAsync(message);
        }
    }
}
