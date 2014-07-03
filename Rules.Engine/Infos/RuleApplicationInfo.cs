using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Domain.Templates;
using Rules.Domain.Vocabulary;
using Rules.Engine.Base;
using Rules.Engine.Functions;
using Rules.Engine.Functions.Builders;
using Rules.Engine.Functions.Templates;
using Rules.Engine.Infos;
using Rules.Engine.Infos.Templates;
using Rules.Engine.Runtime;
using Rules.Engine.Session;
using FunctionInfo = Rules.Engine.Functions.FunctionInfo;

namespace Rules.Engine
{
    internal class RuleApplicationInfo : InfoBase
    {
        private readonly RuleApplicationSpec _ruleApplicationSpec;
        private readonly List<EntityInfo> _entityInfos = new List<EntityInfo>();
        private readonly List<RuleSetInfo> _ruleSetInfos = new List<RuleSetInfo>();

        public RuleApplicationSpec RuleApp { get { return _ruleApplicationSpec; } }
        public List<EntityInfo> EntityInfos { get { return _entityInfos; } }
        public List<RuleSetInfo> RuleSetInfos { get { return _ruleSetInfos; } }

        public RuleApplicationInfo(RuleApplicationSpec ruleApplicationSpec)
        {
            _ruleApplicationSpec = ruleApplicationSpec;
        }

        internal void Compile(Engine engine, FunctionInfo builder)
        {
            // EntityInfo

            foreach (var entity in _ruleApplicationSpec.Entities)
            {
                var entityInfo = new EntityInfo {EntitySpec = entity, RuleSetInfos = new List<RuleSetInfo>()};

                _entityInfos.Add(entityInfo);
                
                var context = new CompileContext {EntityInfo = entityInfo, Context = new EvalInfo(entity.Name)};

                var vocabSpec = _ruleApplicationSpec.Vocabulary;

                CompileVocabulary(vocabSpec, builder, engine, context);

                foreach (var ruleSpec in entity.RuleSets)
                {
                    CompileRuleSet(ruleSpec, builder, engine, context);
                }
            }

            // Independent

            foreach (var ruleSpec in _ruleApplicationSpec.RuleSets)
            {
                CompileRuleSet(ruleSpec, builder, engine, null);
            }
        }

        internal ExecutionResult Execute(Engine engine, WorkingMemory workingMemory, ExecutionResult execResult)
        {   
            foreach (Object item in workingMemory.Values.Values)
            {
                var entity = (Entity) item;
                if (entity != null)
                {
                    entity.StateContainer.Execute(workingMemory);
                }
            }

            return execResult;
        }

        private void CompileRuleSet(RuleSpec ruleSpec, FunctionInfo builder, Engine engine, CompileContext compileContext)
        {
            // Creating a parameter expression.

            // Engine.StateContainerParam = Expression.Parameter(compileContext.EntityInfo.EntitySpec.BoundType, "Context");

            ParameterExpression memoryParam = Engine.WorkingMemoryParam;
            ParameterExpression stateContainerParam = Engine.StateContainerParam;

            // TODO: Add stack context parameter expression

            // Creating an expression to hold a local variable. 
            // ParameterExpression resultParam = Expression.Parameter(typeof(Object), "result");

            var ruleBlocks = new List<Expression>();

            foreach (var rule in ruleSpec.Actions)
            {   
                var compiledBlock = GetCompiledBlock(builder, engine, compileContext, rule);

                GetLocals(compileContext, compiledBlock);

                ruleBlocks.Add(compiledBlock.Code);
            }

            // var variables = ruleBlocks.Where(rb => rb.Type == typeof(BlockExpression)).Select(b => ((BlockExpression)b).Variables);

            var variables = compileContext.Locals.Select(v => (ParameterExpression)v.Value);

            // Creating a method body.
            BlockExpression block = Expression.Block(
                // new []{Engine.StateContainerParam, Engine.WorkingMemoryParam},
                // Adding a local variable.
                variables,
                ruleBlocks);

            LambdaExpression lambda = Expression.Lambda<Action<StateContainer, WorkingMemory>>(block, stateContainerParam, memoryParam);
            
            // EntityInfo
            if (compileContext != null)
            {
                compileContext.EntityInfo.RuleSetInfos.Add(new RuleSetInfo {RuleSpec = ruleSpec, Lambda = lambda});
            }
            // Independent
            else
            {
                this._ruleSetInfos.Add(new RuleSetInfo {RuleSpec = ruleSpec, Lambda = lambda});
            }
        }

        private void CompileVocabulary(VocabularySpec vocabularySpec, FunctionInfo builder, Engine engine, CompileContext compileContext)
        {
            ParameterExpression memoryParam = Engine.WorkingMemoryParam;
            ParameterExpression stateContainerParam = Engine.StateContainerParam;

            if (compileContext != null)
            {
                compileContext.EntityInfo.Vocabulary = new VocabularyInfo();
            }

            foreach (var templateSpec in vocabularySpec.Templates)
            {
                var expressionTemplateAction = new ExpressionTemplateAction(templateSpec);

                var compiledBlock = GetCompiledBlock(builder, engine, compileContext, expressionTemplateAction);

                GetLocals(compileContext, compiledBlock);

                // TODO: Add variables support to templates

                var variables = compileContext.Locals.Select(v => (ParameterExpression) v.Value);

                /*var ruleBlocks = ;

                // Creating a method body.
                BlockExpression block = Expression.Block(
                    // new []{Engine.StateContainerParam, Engine.WorkingMemoryParam},
                    // Adding a local variable.
                    variables,
                    ruleBlocks);*/

                LambdaExpression lambda = Expression.Lambda<Action<StateContainer, WorkingMemory>>(compiledBlock.Code,
                                                                                                   stateContainerParam,
                                                                                                   memoryParam);
                if (compileContext != null)
                {

                    compileContext.EntityInfo.Vocabulary.TemplateInfos.Add(new TemplateInfo { TemplateSpec = templateSpec, Lambda = lambda });
                }
            }
        }

        private static void GetLocals(CompileContext compileContext, CompiledBlock compiledBlock)
        {
            /*
            var variables = compiledBlock.Code.NodeType == ExpressionType.Parameter ? compiledBlock.Code : default(Expression);

            if (variables != null)
            {
                compileContext.Locals.Add(((ParameterExpression) variables).Name, variables);
            }

            variables = compiledBlock.Code.NodeType == ExpressionType.Block ? compiledBlock.Code : default(Expression);

            if (variables != null)
            {
                // var variables = ruleBlocks.Where(rb => rb.Type == typeof(BlockExpression)).Select(b => ((BlockExpression)b).Variables);
                foreach ( var paramExpr in ((BlockExpression) variables).Expressions.Where(b => b.NodeType == ExpressionType.Parameter) )
                {
                    compileContext.Locals.Add(((ParameterExpression)paramExpr).Name, paramExpr);    
                }
            }*/

            if (compiledBlock.Variables == null)
            {
                return;
            }

            foreach (var variable in compiledBlock.Variables)
            {
                compileContext.Locals.Add(variable.Name, variable);
            }
        }

        internal CompiledBlock GetCompiledBlock(FunctionInfo builder, Engine engine, CompileContext compileContext,
                                                      Rule rule)
        {   
            var functionBuilder = builder.GetFunctionBuilder(rule, compileContext);

            var functionInfo = functionBuilder.GetFunctionBuilder(rule, compileContext);

            if ((functionBuilder as SetValueActionFunctionBuilder) != null)
            {   
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((SetValueActionFunction)functionInfo).Info);

                return compiledBlock;    
            }
            if ((functionBuilder as SimpleRuleSetFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((SimpleRuleSetFunction)functionInfo).Info);

                return compiledBlock;    
            }
            if ((functionBuilder as WhileRuleSetFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((WhileRuleSetFunction)functionInfo).Info);

                return compiledBlock;
            }
            if ((functionBuilder as AddCollectionMemberActionFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((AddCollectionMemberFunction)functionInfo).Info);

                return compiledBlock;
            }
            if ((functionBuilder as DeclareVariableFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((DeclareVariableFunction)functionInfo).Info);

                return compiledBlock;
            }
            if ((functionBuilder as ExpressionTemplateFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((ExpressionTemplateFunction)functionInfo).Info);

                return compiledBlock;
            }
            if ((functionBuilder as FunctionNodeFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((FunctionNodeFunction)functionInfo).Info);

                return compiledBlock;
            }
            if ((functionBuilder as SendMailActionFunctionBuilder) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionInfo.BuildInfo(engine, compiledBlock, ((SendMailFunction)functionInfo).Info);

                return compiledBlock;
            }

            throw new InvalidOperationException("Unknown code construct.");
        }
    }
}
