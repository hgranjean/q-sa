using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SurveyWeb.Services
{
    internal class MailService
    {
        private static SmtpClient s_client;

        static MailService()
        {
            s_client = new SmtpClient();   
        }

        public void SendEmail(string from, string email, string subject, string body, bool isHtml = true)
        {
            var message = new MailMessage(from, email);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            s_client.Send(message);
        }
    }
}