using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Functions;

namespace Rules.Engine.Infos
{
    internal class SendMailActionInfo : IInfo
    {
        public IInfo FromInfo { get; set; }
        public IInfo ToInfo { get; set; }
        public IInfo SubjectInfo { get; set; }
        public IInfo BodyInfo { get; set; }
        public String Name { get; set; }
        public CompileContext Context { get; set; }
    }
}
