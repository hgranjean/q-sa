using System;
using Rules.Engine.Infos;
using System.Linq.Expressions;

namespace Rules.Engine.Functions
{
    internal class AddCollectionMemberFunction : FunctionBuilder
    {
        public AddCollectionMemberActionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (AddCollectionMemberActionInfo)info;

            var collectionInfo = engine.GetExpressionForValue(actionInfo.Context, actionInfo.TargetInfo, true);

            if (collectionInfo == null)
            {
                throw new Exception("Cannot add member to non-compatible collection. Expecting collection of type List<>");
            }
            
            var addMemberInfo = collectionInfo.Type.GetMethod("Add");

            var argumentInfo = collectionInfo.Type.GenericTypeArguments[0];
            
            block.Code = Expression.Call(collectionInfo, addMemberInfo, Expression.New(argumentInfo));
        }
    }
}
