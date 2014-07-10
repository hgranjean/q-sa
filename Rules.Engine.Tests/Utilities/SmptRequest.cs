using System;

namespace Rules.Engine.Tests.Utilities
{
    internal sealed class SmtpRequest
    {
        public readonly SmtpRequestType Code;
        public readonly string Message;

        public SmtpRequest(string request)
        {
            if (String.IsNullOrWhiteSpace(request))
            {
                Code = SmtpRequestType.ERROR;
                Message = request;
                return;
            }

            string code = request.Substring(0, 4).ToUpper();
            if (Enum.TryParse(code, out Code))
            {
                if (request.Length > 4)
                {
                    Message = request.Substring(5);
                }
            }
            else
            {
                Code = SmtpRequestType.ERROR;
                Message = request;
            }
        }
    }

    internal enum SmtpRequestType
    {
        HELO,
        EHLO,
        MAIL,
        RCPT,
        DATA,
        QUIT,
        RSET,
        NOOP,
        HELP,
        ERROR
    }
}
