using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Security;

namespace Atum.Utility
{
    public class EmailHelper
    {
        private static Regex ValidEmailRegex = CreateValidEmailRegex();

        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }

        public static bool IsValidEmail(string emailAddress)
        {
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }

        /// <summary>
        /// Given email address, ie. joe@domain.com, returns the domain name (domain.com)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDomainNameFromEmail(string value)
        {
            var mailAddress = new MailAddress(value);
                        
            return mailAddress.Host;
        }

        /// <summary>
        /// Given the host address (www.domain.com), returns the domain name (domain.com)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDomainNameFromHost(string value)
        {
            var result = value == "localhost" ? "localhost.com" : value.Replace("www.", string.Empty);

            return result;
        }

        public static string GenerateToken(string username, int validityInHours = 24, IEnumerable<string> userData = null)
        {
            var formsTicket = new FormsAuthenticationTicket(
                1,
                username,
                DateTime.Now,
                DateTime.Now.AddHours(validityInHours),
                true,
                string.Join("|", userData)
            );

            // encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(formsTicket);

            return encryptedTicket;
        }

        public static string GetUsernameFromToken(string encryptedTicket)
        {
            FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(encryptedTicket);

            // split the user data back apart
            string[] userData = formsTicket.UserData.Split(new string[] { "|" }, StringSplitOptions.None);

            // verify that the username in the ticket matches the username that was sent with the request
            return formsTicket.Name;
        }
        
        public static IEnumerable<KeyValuePair<string,string>> GetUserdataFromToken(string encryptedTicket)
        {
            FormsAuthenticationTicket formsTicket = FormsAuthentication.Decrypt(encryptedTicket);

            // split the user data back apart
            string[] userData = formsTicket.UserData.Split(new string[] { "|" }, StringSplitOptions.None);

            foreach (var str in userData)
            {
                var key = str.Split('=')[0];
                var value = str.Split('=')[1];
                
                yield return new KeyValuePair<string, string>(key, value);
            }
        }
    }
}
