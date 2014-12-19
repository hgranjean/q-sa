using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Linq.Dynamic;

namespace Rules.Engine.Tests
{
    [TestFixture]
    public class LambdaTests
    {
        [Test]
        public void TestCollAction()
        {
            
                var list = new List<Rules.Engine.Tests.CollectionAggregateTests.Entity1>();
                list.Add(new Rules.Engine.Tests.CollectionAggregateTests.Entity1 { Field1 = "3" });
                list.Add(new Rules.Engine.Tests.CollectionAggregateTests.Entity1 { Field1 = "1" });
                list.Add(new Rules.Engine.Tests.CollectionAggregateTests.Entity1 { Field1 = "2" });

                // Min(list, t t=> t.Field1) =>
                var min1 = Queryable.Min(Queryable.AsQueryable(list), t => t.Field1);

                //var min2 = System.Linq.Dynamic.DynamicQueryable.Min(list, t => t.Field1);
            
        }

        [Test]
        public void TestLambdaConditional()
        {
            // Creating a parameter expression.
            ParameterExpression value = Expression.Parameter(typeof(int), "value");

            // Creating an expression to hold a local variable. 
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            // Creating a label to jump to from a loop.
            LabelTarget label = Expression.Label(typeof(int));

            // Creating a method body.
            BlockExpression block = Expression.Block(
                // Adding a local variable. 
                new[] { result },
                    // Assigning a constant to a local variable: result = 1
                    Expression.Assign(result, Expression.Constant(1)),
                    // Adding a loop.
                    Expression.Loop(
                    // Adding a conditional block into the loop.
                    Expression.IfThenElse(
                    // Condition: value > 1
                    Expression.GreaterThan(value, Expression.Constant(1)),
                    // If true: result *= value --
                    Expression.MultiplyAssign(result,
                    Expression.PostDecrementAssign(value)),
                        // If false, exit the loop and go to the label.
                    Expression.Break(label, result)
                ),
            // Label to jump to.
            label
            )
            );
            
            // Compile and execute an expression tree. 
            int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
            
            Console.WriteLine(factorial);
        }
    }
}
