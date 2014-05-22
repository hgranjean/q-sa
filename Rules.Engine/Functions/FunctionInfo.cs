using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Domain.Templates;
using Rules.Engine.Functions.Builders;
using Rules.Engine.Functions.Templates;
using Rules.Engine.Infos;

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

            throw new Exception("unknown func builder");
        }
    }
}
