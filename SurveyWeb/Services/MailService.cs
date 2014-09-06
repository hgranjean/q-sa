using SurveyWeb.Repository;
using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net.Mail;
using System.Xml.Linq;

namespace SurveyWeb.Services
{   public enum EmailTemplate
    {
        Invitation,
        ResetPassword,
        EventAssigned
    }

    public class MailService
    {
        private static readonly Lazy<SmtpClient> _smtpClient;
        private readonly ISurveyStore _store;

        public MailService(ISurveyStore store)
        {
            _store = store;
        }

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

        public XDocument GetEmailTemplate(EmailTemplate template)
        {
            Contract.Assert(_store != null);
            
            var appPath = _store.GetPath(StoreType.Emails);

            var emailFileName = string.Empty;
            
            if (template == EmailTemplate.Invitation)
            {
                emailFileName = Path.Combine(appPath, "InvitationEmail.xml");
            }
            else if (template == EmailTemplate.ResetPassword)
            {
                emailFileName = Path.Combine(appPath, "ResetPassword.xml");
            }
            else if (template == EmailTemplate.EventAssigned)
            {
                emailFileName = Path.Combine(appPath, "EventAssigned.xml");
            }
            
            return XDocument.Load(emailFileName);
        }
    }
}