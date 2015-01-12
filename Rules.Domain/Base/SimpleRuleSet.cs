using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rules.Domain
{
    [XmlInclude(typeof(SetValueAction))]
    [XmlInclude(typeof(SendMailAction))]
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
