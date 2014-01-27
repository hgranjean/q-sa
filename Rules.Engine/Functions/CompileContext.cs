using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Base;

namespace Rules.Engine.Functions
{
    internal class CompileContext
    {
        public EvalInfo Context { get; set; }

        public EntityInfo EntityInfo { get; set; }
    }
}
