using System.Net;
using System.Net.Mail;
using vn.edu.fpt.service.Interfaces;

namespace vn.edu.fpt.service.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpHost = _configuration["EmailSettings:SmtpHost"];
            var smtpPortString = _configuration["EmailSettings:SmtpPort"];
            var smtpUser = _configuration["EmailSettings:SmtpUser"];
            var smtpPass = _configuration["EmailSettings:SmtpPass"];
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? smtpUser;
            var fromName = _configuration["EmailSettings:FromName"] ?? "Finding Jobs";

            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUser) || string.IsNullOrEmpty(smtpPass) || 
                smtpUser == "your-email@gmail.com")
            {
                _logger.LogWarning("SMTP settings are not configured or still using placeholders. Logging email to console.");
                _logger.LogInformation($"[EMAIL SIMULATION] To: {email}, Subject: {subject}, Message: {message}");
                return;
            }

            int smtpPort = int.TryParse(smtpPortString, out var port) ? port : 587;

            try
            {
                using (var client = new SmtpClient(smtpHost, smtpPort))
                {
                    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail ?? smtpUser ?? "noreply@rjms.com", fromName),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(email);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent successfully to {email}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email to {email}. Continuing registration process.");
                _logger.LogInformation($"[EMAIL FALLBACK] To: {email}, Subject: {subject}, Message: {message}");
            }
        }
    }
}
