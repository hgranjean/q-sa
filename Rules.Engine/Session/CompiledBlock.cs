using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Engine
{
    public class CompiledBlock
    {
        public Expression Code { get; set; }
        public List<ParameterExpression> Variables { get; set; }
    }
}
