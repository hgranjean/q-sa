using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class InMemoryRuleApplicationReference : RuleApplicationReference
    {
        public InMemoryRuleApplicationReference(RuleApplicationSpec ruleApplicationSpec) : base(ruleApplicationSpec)
        {}
    }
}
