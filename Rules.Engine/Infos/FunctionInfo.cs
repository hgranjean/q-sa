using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;

namespace Rules.Engine.Infos
{
    internal class FunctionInfo : IInfo
    {
        public FunctionInfo(Rule rule)
        {
            this.Rule = rule;
        }

        public Rule Rule { get; private set; }
    }
}
