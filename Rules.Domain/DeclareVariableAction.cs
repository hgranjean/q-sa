using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class DeclareVariableAction : RuleFunctionBase
    {  
        public new String Name { get { return base.Name; } set { base.Name = value; } }
        public String Value { get; set; }
        public String ValueType { get; set; }
    }
}
