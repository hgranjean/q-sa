using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Atum.Common;

namespace Rules.Domain
{
    [XmlInclude(typeof(SimpleRuleSet))]
    [XmlInclude(typeof(WhileRuleSet))]
    [XmlInclude(typeof(SetValueAction))]
    [XmlInclude(typeof(AddCollectionMemberAction))]
    [XmlInclude(typeof(DeclareVariableAction))]
    public class Rule : RuleObjectBase
    {
        public String Xmiid { get; set; }
        public SerializableDictionary<Object, Object> MetadataAttributes { get; set; }

        public Rule()
        {
        }

        public Rule(String xmiid)
        {
            Xmiid = xmiid;
        }
    }

}
