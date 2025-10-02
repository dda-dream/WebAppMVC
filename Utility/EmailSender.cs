namespace WebAppMVC.Utility
{
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Options;
    using System.Net;
    using System.Net.Mail;

    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IConfiguration _configuration;
        private EmailSettings _emailSettings;
        
        public EmailSender(ILogger<EmailSender> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _emailSettings = _configuration.GetSection("Smtp").Get<EmailSettings>();
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var smtpServer = _emailSettings.Server;
            var smtpPort = int.Parse(_emailSettings.Port);
            var fromEmail = _emailSettings.FromEmail;

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential("", ""),  // Нет аутентификации для Papercut
                EnableSsl = false  // Отключить SSL для локального сервера
            };

            var mailMessage = new MailMessage(fromEmail, email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email отправлено на {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка отправки email на {email}");
                throw;
            }
        }
    }
}
