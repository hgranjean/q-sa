using Rules.Engine.Infos;

namespace Rules.Engine.Functions
{
    internal class FunctionNodeFunction : FunctionBuilder
    {
        public Infos.FunctionInfo Info { get; set; }

        public override void BuildInfo(Engine engine, CompiledBlock block, IInfo info)
        {
            var actionInfo = (DeclareVariableActionInfo) info;
            
            if (actionInfo.ValueInfo != null)
            {
                var primitiveValueExpr = DeclareVariableFunction.ConvertToPrimitiveType(engine
                                            .GetExpressionForValue(actionInfo.Context, actionInfo.ValueInfo), actionInfo.ValueType);
                
                block.Code = primitiveValueExpr;
            }
        }
    }
}
