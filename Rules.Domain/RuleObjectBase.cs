using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    /**
     * 
    * just holding the name for a Rule
    * might have more basic attributes
    *
    */
    public class RuleObjectBase
    {
        public String Name { get; set; }
        public RuleObjectBase Parent { get; set; }
    }
}
