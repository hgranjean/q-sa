using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain.Base;

namespace Rules.Domain
{
    public class RuleApplicationSpec : RuleObjectBase
    {
	    public AuthoringSettings Settings { get; set; }
        public const int FeatureVersion = 1;
	    public String Imports { get; set; }
	    // public List<ClassNode> vocabulary;
	    // private List<UserFunctionLibrary> functionLibrary;
        public List<EntitySpec> Entities { get; set; } 
	    public List<RuleSpecification> RuleSets { get; set; }
	    // public List<SchemaEndpoint> schemaEndpoints;
	    // private String strategyTemplate;
        public string Name { get { return base.Name; } set { base.Name = value; } }

        public RuleApplicationSpec()
        {
            Settings = new AuthoringSettings();
            RuleSets = new List<RuleSpecification>();
            Entities = new List<EntitySpec>();
        }
    }
}
