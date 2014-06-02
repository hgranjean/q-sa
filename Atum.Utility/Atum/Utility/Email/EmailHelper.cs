using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Utility
{
    public class EmailHelper
    {
        public static string GetDomainName(string value)
        {
            var mailAddress = new MailAddress(value);

            return mailAddress.Host;
        }
    }
}
