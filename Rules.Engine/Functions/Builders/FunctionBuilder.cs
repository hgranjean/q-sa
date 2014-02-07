using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Functions;
using Rules.Engine.Infos;

namespace Rules.Engine
{
    internal class FunctionBuilder
    {
        public virtual void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
        }

        public virtual FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            return null;
        }
    }
}
