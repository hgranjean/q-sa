using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum.Domain.Basis;

namespace Rules.Domain
{
    public class SimpleRuleSet : Rule
    {
        public String Condition { get; set; }
        public List<Rule> Rules { get; set; } 

        public SimpleRuleSet()
        {
            this.Rules = new List<Rule>();
        }
    }
}
