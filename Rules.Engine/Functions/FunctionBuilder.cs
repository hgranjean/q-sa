using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class FunctionBuilder
    {
        public virtual FunctionBuilderBase GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            throw new NotImplementedException("Should implement.");
        }
    }
}
