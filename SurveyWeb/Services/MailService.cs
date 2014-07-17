using System;
using System.Configuration;
using System.Net.Mail;

namespace SurveyWeb.Services
{
    internal class MailService
    {
        private static readonly Lazy<SmtpClient> _smtpClient;

        static MailService()
        {
            _smtpClient = new Lazy<SmtpClient>(() => new SmtpClient());   
        }

        public void SendEmail(string from, string email, string subject, string body, bool isHtml = true, string domainName = null)
        {
            var whiteLabel = ConfigurationManager.AppSettings["WhiteLabel"];

            body = body.Replace("{{DOMAIN_NAME}}", domainName).Replace("{{WHITE_LABEL}}", whiteLabel);

            var message = new MailMessage(from, email);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            
            _smtpClient.Value.Send(message);
        }
    }
}