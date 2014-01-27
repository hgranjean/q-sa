using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Functions;

namespace Rules.Engine.Infos
{
    internal class SetValueActionInfo : IInfo
    {
        public IInfo ValueInfo { get; set; }
        public IInfo TargetInfo { get; set; }
        public String Name { get; set; }
        public CompileContext Context { get; set; }
    }
}
