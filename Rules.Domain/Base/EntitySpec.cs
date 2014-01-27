using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class EntitySpec : RuleObjectBase
    {
        public Type BoundType { get; internal set; }
        public String Name { get { return base.Name; } set { base.Name = value; } }

        public EntitySpec(String name, Type boundType)
        {
            this.Name = name;
            this.BoundType = boundType;
            this.RuleSets = new List<RuleSpecification>();
        }

        public List<RuleSpecification> RuleSets { get; set; }
    }
}
