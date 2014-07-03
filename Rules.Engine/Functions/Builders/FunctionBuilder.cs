using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        protected static Type Type(Expression lhs)
        {
            Type type = null;

            if (lhs is ParameterExpression)
            {
                type = lhs.Type;
            }
            else if (lhs is IndexExpression)
            {
                type = typeof (object);
            }
            else if (lhs is UnaryExpression)
            {
                type = ((UnaryExpression) lhs).Operand.Type;
            }
            else
            {
                type = lhs.Type;
            }
            return type;
        }
    }
}
