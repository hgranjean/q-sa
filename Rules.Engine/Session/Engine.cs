using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
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
        public readonly static ParameterExpression StateContainerParam = Expression.Parameter(typeof(StateContainer), "stateContainer");

        internal RuleApplicationInfo RuleApplicationInfo { get; private set; }

        public Engine(RuleApplicationInfo ruleApplicationInfo)
        {
            this.RuleApplicationInfo = ruleApplicationInfo;
        }
        
        internal Expression GetExpressionForValue(CompileContext context, IInfo info)
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

                    Type targetType = context.EntityInfo.EntitySpec.BoundType;

                    if (eval.Contains("=="))
                    {
                        var binder = Binder.GetMember(CSharpBinderFlags.None, "GetMember", typeof(StateContainer),
                            new [] {CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)});

                        /*Func<dynamic, dynamic, dynamic> f =
                                            Expression.Lambda<Func<object, object, object>>(
                                            Expression.Dynamic(binder, typeof(object), x, y),
                                new[] { x, y }
                            ).Compile();*/

                        var props = new [] {new DynamicProperty("Context", typeof(StateContainer))};

                        //System.Linq.Dynamic.DynamicExpression.CreateClass(props)
                        var param = Expression.Variable(typeof(StateContainer), "Context");

                        var paramValue = Expression.Dynamic(
                            new StateContainerBinder(), typeof(object), Expression.Parameter(typeof(StateContainer), "Context"));

                        Func<dynamic, dynamic> f =
                            Expression.Lambda<Func<dynamic, dynamic>>(
                                Expression.Dynamic(binder, typeof(StateContainer), param),
                                new[] { param }
                        ).Compile();

                        var e = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[]{param}, typeof(bool), eval, new[]{f});

                        return e.Body;
                    }
                    else
                    {
                        Expression propertyExpression = Expression.Property(Expression.Convert(keyExpression, typeof(StateContainer)), "Item", Expression.Constant(eval));

                        return propertyExpression;
                    }
                    
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
