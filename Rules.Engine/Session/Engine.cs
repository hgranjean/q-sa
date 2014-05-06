using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;
using Rules.Engine.Functions;
using Rules.Engine.Infos;
using Rules.Engine.Session;

namespace Rules.Engine
{
    internal class Engine
    {
        public const string MemoryLiteral = "__memory";
        public const string ContextLiteral = "Context";

        public readonly static ParameterExpression WorkingMemoryParam = Expression.Parameter(typeof(WorkingMemory), MemoryLiteral);
        public readonly static ParameterExpression StateContainerParam = Expression.Parameter(typeof(StateContainer), ContextLiteral);
        
        internal RuleApplicationInfo RuleApplicationInfo { get; private set; }

        public Engine(RuleApplicationInfo ruleApplicationInfo)
        {
            this.RuleApplicationInfo = ruleApplicationInfo;
        }

        // TODO: Move requiresSpecType out and make a separate method
        internal Expression GetExpressionForValue(CompileContext context, IInfo info, bool requiresSpecificType = false)
        {
            Object eval = ((EvalInfo)info).Eval;

            if (eval is String && Char.IsLetter(((String)eval)[0]))
            {
                if (context != null)
                {   
                    var externals = new Dictionary<string, object>();
                    
                    AddContextProps(context, externals);

                    AddContext(context, externals);

                    AddLocals(context, externals);

                    /* parse within the context of statecontainer */

                    var lambdaExpression = System.Linq.Dynamic.DynamicExpression.ParseLambda(
                                new[] { StateContainerParam }, typeof(object), eval.ToString(), externals);

                    return UnwindExpression(lambdaExpression, requiresSpecificType);
                }
                else
                {
                    //left part of lambda, p
                    var parameter = WorkingMemoryParam;

                    //right part
                    //p.Values
                    Expression left = Expression.Property(parameter, "Values");
                    
                    //p.Values.Item[info.Name]
                    Expression keyExpression = Expression.Property(left, "Item",
                                                                   new Expression[] { Expression.Constant(eval) });

                    return keyExpression;
                }
            }
            else
            {
                return Expression.Constant(eval);
            }
        }

        private static Expression UnwindExpression(LambdaExpression lambdaExpression, bool requiresSpecificType)
        {
            if (lambdaExpression.Body is IndexExpression)
            {
                Expression expression = lambdaExpression.Body;
                return expression;
            }

            Expression operand = lambdaExpression.Body;
            while (operand is UnaryExpression)
            {
                operand = ((UnaryExpression) operand).Operand;

                if (requiresSpecificType && operand.Type != typeof (object))
                    break;
            }

            return operand;
        }

        private static void AddContextProps(CompileContext context, IDictionary<string, object> externals)
        {
            var props = context.EntityInfo.EntitySpec.BoundType.GetProperties()
                       .Select(p => p)
                       .ToDictionary<PropertyInfo, string, object>(item => item.Name, item => item);


            foreach (var key in props.ToDictionary(item => item.Key, item => item.Value))
            {
                externals.Add(key);
            }
        }

        private static void AddContext(CompileContext context, IDictionary<string, object> externals)
        {
            externals.Add(ContextLiteral, context.EntityInfo.EntitySpec.BoundType);
        }

        private static void AddLocals(CompileContext context, IDictionary<string, object> externals)
        {
            foreach (var local in context.Locals)
            {
                externals.Add(local.Key, local.Value);
            }
        }
    }
}
