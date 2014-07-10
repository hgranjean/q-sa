using System;
using Rules.Domain;
using Rules.Domain.Templates;
using Rules.Engine.Functions.Builders;

namespace Rules.Engine.Functions
{
    internal class FunctionInfo
    {
        public virtual FunctionBuilder GetFunctionBuilder(Rule rule, CompileContext compileContext)
        {
            if (rule is SetValueAction)
            {
                return new SetValueActionFunctionBuilder();
            }
            if (rule is WhileRuleSet) // While should be before SimpleRuleSet
            {
                return new WhileRuleSetFunctionBuilder();
            }
            if (rule is SimpleRuleSet)
            {
                return new SimpleRuleSetFunctionBuilder();
            }
            if (rule is AddCollectionMemberAction)
            {
                return new AddCollectionMemberActionFunctionBuilder();
            }
            if (rule is DeclareVariableAction)
            {
                return new DeclareVariableFunctionBuilder();
            }
            if (rule is ExpressionTemplateAction)
            {
                return new ExpressionTemplateFunctionBuilder();
            }
            if (rule is FunctionNode)
            {
                return new FunctionNodeFunctionBuilder();
            }
            if (rule is SendMailAction)
            {
                return new SendMailActionFunctionBuilder();
            }

            throw new Exception("unknown func builder");
        }
    }
}
