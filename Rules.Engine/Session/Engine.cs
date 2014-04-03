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
    internal enum ValueType
    {
        Setter,
        Getter
    }

    internal class Engine
    {
        public readonly static ParameterExpression WorkingMemoryParam = Expression.Parameter(typeof(WorkingMemory), "memory");
        public readonly static ParameterExpression StateContainerParam = Expression.Parameter(typeof(StateContainer), "Context");

        internal RuleApplicationInfo RuleApplicationInfo { get; private set; }

        public Engine(RuleApplicationInfo ruleApplicationInfo)
        {
            this.RuleApplicationInfo = ruleApplicationInfo;
        }

        // TODO: Move requiresSpecType out and make a separate method
        internal Expression GetExpressionForValue(CompileContext context, IInfo info, ValueType? isSetter = ValueType.Getter, bool requiresSpecificType = false)
        {
            Object eval = ((EvalInfo)info).Eval;

            if (eval is String && Char.IsLetter(((String)eval)[0]))
            {
                if (context != null)
                {   
                    var externals = GetContextProps(context).ToDictionary(item => item.Key, item => item.Value);

                    AddContext(context, externals);

                    AddLocals(context, externals);

                    /* parse within the context of statecontainer */

                    var e = System.Linq.Dynamic.DynamicExpression.ParseLambda(new[] { StateContainerParam }, typeof(object), eval.ToString(), externals);
                    
                    if (e.Body is IndexExpression)
                    {
                        return e.Body;
                    }

                    var operand = e.Body;
                    while (operand is UnaryExpression)
                    {
                        operand = ((UnaryExpression)operand).Operand;

                        if (requiresSpecificType && operand.Type != typeof (object))
                            break;
                    }

                    //if (isSetter == ValueType.Setter)
                    //{
                    //    return Expression.Convert(operand, typeof(object));
                    //}

                    return operand;
                }
                else
                {
                    //left part of lambda, p
                    var parameter = WorkingMemoryParam;

                    //right part
                    //p.Values
                    Expression left = Expression.Property(parameter, "Values");

                    var method = typeof(IDictionary<Object, Object>).GetMethod("ContainsKey");

                    //p.Values.ContainsKey(info.Name);
                    // Expression containsExpression = Expression.Call(left, method, Expression.Constant(eval));

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

        private static void AddLocals(CompileContext context, Dictionary<string, object> externals)
        {
            foreach (var local in context.Locals)
            {
                externals.Add(local.Key, local.Value);
            }
        }

        private static void AddContext(CompileContext context, Dictionary<string, object> externals)
        {
            externals.Add("Context", context.EntityInfo.EntitySpec.BoundType);
        }

        private static IEnumerable<KeyValuePair<string, object>> GetContextProps(CompileContext context)
        {
            return context.EntityInfo.EntitySpec.BoundType.GetProperties()
                       .Select(p => p)
                       .ToDictionary<PropertyInfo, string, object>(item => item.Name, item => item);
        }
    }
}
