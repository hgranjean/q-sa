using System;

namespace Rules.Domain
{
    public class SendMailAction : RuleFunctionBase
    {
        public String From { get; set; }
        public String To { get; set; }
        public String Subject { get; set; }
        public String Body { get; set; }
        public String Server { get; set; }
    }
}
