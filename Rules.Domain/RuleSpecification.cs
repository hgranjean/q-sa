using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{

    public class RuleSpecification : RuleObjectBase
    {
        public String Specification { get; set; }
        public List<Rule> Actions { get; set; }
        public List<Template> VocabularyTemplates { get; set; }
        public String UmlFileName { get; set; }

        public RuleSpecification()
        {
            Actions = new List<Rule>();
            VocabularyTemplates = new List<Template>();
        }

        /**
	    * Creates an instance of rule specification for the given text
	    * 
	    * @param specification
	    */

        public RuleSpecification(String specification) : this()
        {
            this.Specification = specification;
        }
    }
}
