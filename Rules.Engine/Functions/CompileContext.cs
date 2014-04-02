using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Base;

namespace Rules.Engine.Functions
{
    internal class CompileContext
    {
        public EvalInfo Context { get; set; }

        public EntityInfo EntityInfo { get; set; }

        public readonly Dictionary<string, Expression> Locals = new Dictionary<string, Expression>();
    }
}
