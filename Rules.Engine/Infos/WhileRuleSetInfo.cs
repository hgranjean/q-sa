using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Functions;

namespace Rules.Engine.Infos
{
    internal class WhileRuleSetInfo : IInfo
    {
        public IInfo ConditionInfo { get; set; }
        public List<IInfo> TargetInfo { get; set; }
        public String Name { get; set; }
        public CompileContext Context { get; set; }
    }
}
