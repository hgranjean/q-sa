using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Base;
using Rules.Engine.Session;

namespace Rules.Engine.Base
{
    internal class EntityInfo : InfoBase
    {
        public EntitySpec EntitySpec { get; set; }
        public List<RuleSetInfo> RuleSetInfos { get; set; }

        internal void Execute(StateContainer stateContainer, WorkingMemory workingMemory)
        {
            foreach (var ruleSet in this.RuleSetInfos)
            {   
                ruleSet.Lambda.Compile().DynamicInvoke(stateContainer, workingMemory);
            }
        }
    }
}
