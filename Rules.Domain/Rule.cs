using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class Rule : RuleObjectBase
    {
        public String Xmiid { get; set; }
        public Dictionary<Object, Object> MetadataAttributes { get; set; }

        public Rule()
        {
        }

        public Rule(String xmiid)
        {
            Xmiid = xmiid;
        }
    }

}
