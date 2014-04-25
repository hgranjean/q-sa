using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Rules.Domain
{
    public class EntitySpec : RuleObjectBase
    {
        [XmlIgnore]
        public Type BoundType { get; set; }
        public String Name { get { return base.Name; } set { base.Name = value; } }

        protected EntitySpec()
        {
        }

        public EntitySpec(String name, Type boundType)
        {
            this.Name = name;
            this.BoundType = boundType;
            this.RuleSets = new List<RuleSpec>();
        }

        public List<RuleSpec> RuleSets { get; set; }
    }
}
