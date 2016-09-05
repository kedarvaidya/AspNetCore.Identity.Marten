using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AspNetCore.Identity.Marten.MvcSample.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class LoggingAuthMessageSender : IEmailSender, ISmsSender
    {
        private ILogger<LoggingAuthMessageSender> _logger;

        public LoggingAuthMessageSender(ILogger<LoggingAuthMessageSender> logger)
        {
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            using (_logger.BeginScope(nameof(SendEmailAsync)))
            {
                _logger.LogInformation("Email");
                _logger.LogInformation($"To: {email}");
                _logger.LogInformation($"Subject: {subject}");
                _logger.LogInformation($"Message: {message}");
            }

            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            using (_logger.BeginScope(nameof(SendSmsAsync)))
            {
                _logger.LogInformation("Sms");
                _logger.LogInformation($"To: {number}");
                _logger.LogInformation($"Message: {message}");
            }

            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
