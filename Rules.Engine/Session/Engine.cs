using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
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
        public readonly static ParameterExpression WorkingMemoryParam = Expression.Parameter(typeof (WorkingMemory), "memory");
        public readonly static ParameterExpression StateContainerParam = Expression.Parameter(typeof(StateContainer), "Context");

        internal RuleApplicationInfo RuleApplicationInfo { get; private set; }

        public Engine(RuleApplicationInfo ruleApplicationInfo)
        {
            this.RuleApplicationInfo = ruleApplicationInfo;
        }
        
        internal Expression GetExpressionForValue(CompileContext context, IInfo info, bool isSetter = false)
        {
            // TODO: Distinguish between lhs and rhs

            String eval = ((EvalInfo) info).Eval;
            
            if (Char.IsLetter(eval[0]))
            {
                if (context != null)
                {
                    //left part of lambda, p
                    var keyExpression = StateContainerParam;
                    
                    //p.Values.Item[info.Name]
                    //Expression keyExpression = Expression.Property(parameter, contextEval.Eval);
                    
                    var param = Expression.Variable(context.EntityInfo.EntitySpec.BoundType, "Context");

                    var e = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { param }, typeof(object), eval, new[] { param });

                    if (isSetter)
                    {
                        return ((UnaryExpression) e.Body).Operand;
                    }

                    return e.Body;
                }
                else
                {
                    //left part of lambda, p
                    var parameter = WorkingMemoryParam;

                    //right part
                    //p.Values
                    Expression left = Expression.Property(parameter, "Values");

                    var method = typeof (IDictionary<Object, Object>).GetMethod("ContainsKey");

                    //p.Values.ContainsKey(info.Name);
                    // Expression containsExpression = Expression.Call(left, method, Expression.Constant(eval));

                    //p.Values.Item[info.Name]
                    Expression keyExpression = Expression.Property(left, "Item",
                                                                   new Expression[] {Expression.Constant(eval)});

                    return keyExpression;
                }
            }
            else
            {
                return Expression.Constant(eval);
            }
            

            /*
            //"somevalue"
            var right = Expression.Constant(value);
            //{p => IIF(p.Attributes.ContainsKey("Brand"), (p.Attributes.Item[info.Name] == "somevalue"), False)}
            Expression operation = Expression.Condition(
                           containsExpression,
                           Expression.MakeBinary(expressionType[op], keyExpression, right), 
                           Expression.Constant(false));
            var lambda = Expression.Lambda<Func<Object, bool>>(operation, parameter);*/
        }
    }
}
