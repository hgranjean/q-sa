using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public abstract class RuleApplicationReference
    {
        public RuleApplicationSpec RuleApplicationSpec { get; private set; }

        protected RuleApplicationReference()
        {
        }

        public RuleApplicationReference(RuleApplicationSpec ruleApplicationSpec)
        {
            RuleApplicationSpec = ruleApplicationSpec;
        }
    }
}
