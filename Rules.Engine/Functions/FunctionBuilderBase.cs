using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Infos;

namespace Rules.Engine
{
    internal class FunctionBuilderBase
    {
        public virtual void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
        }
    }
}
