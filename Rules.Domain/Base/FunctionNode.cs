using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    public class FunctionNode : Rule
    {
        public string Expression { get; set; }
        public string ValueType { get; set; }

        public static FunctionNode Create(string expression)
        {
            return new FunctionNode {Expression = expression};
        }

        public string FunctionName { get; set; }
    }
}
