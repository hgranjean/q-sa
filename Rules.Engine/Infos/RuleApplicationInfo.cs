using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Base;
using Rules.Engine.Functions;
using Rules.Engine.Functions.Builders;
using Rules.Engine.Runtime;
using Rules.Engine.Session;

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

        internal void Compile(Engine engine, FunctionBuilder builder)
        {
            // EntityInfo

            foreach (var entity in _ruleApplicationSpec.Entities)
            {
                var entityInfo = new EntityInfo {EntitySpec = entity, RuleSetInfos = new List<RuleSetInfo>()};

                _entityInfos.Add(entityInfo);

                var context = new CompileContext {EntityInfo = entityInfo, Context = new EvalInfo(entity.Name)};

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

        private void CompileRuleSet(RuleSpecification ruleSpec, FunctionBuilder builder, Engine engine, CompileContext compileContext)
        {
            // Creating a parameter expression.
            ParameterExpression memoryParam = Engine.WorkingMemoryParam;
            ParameterExpression stateContainerParam = Engine.StateContainerParam;

            // Creating an expression to hold a local variable. 
            ParameterExpression resultParam = Expression.Parameter(typeof(Object), "result");

            var ruleBlocks = new List<Expression>();

            foreach (var rule in ruleSpec.Actions)
            {   
                var compiledBlock = GetCompiledBlock(builder, engine, compileContext, rule);

                ruleBlocks.Add(compiledBlock.Code);
            }

            // Creating a method body.
            BlockExpression block = Expression.Block(
                // Adding a local variable. 
                new[] { resultParam }, ruleBlocks);

            LambdaExpression lambda = Expression.Lambda<Func<StateContainer, WorkingMemory, Object>>(block, stateContainerParam, memoryParam);

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

        internal CompiledBlock GetCompiledBlock(FunctionBuilder builder, Engine engine, CompileContext compileContext,
                                                      Rule rule)
        {
            var functionBuilder = builder.GetFunctionBuilder(rule, compileContext);

            if ((functionBuilder as SetValueActionFunction) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionBuilder.BuildInfo(engine, compiledBlock, ((SetValueActionFunction) functionBuilder).Info);

                return compiledBlock;    
            }
            else if ((functionBuilder as SimpleRuleSetFunction) != null)
            {
                var compiledBlock = new CompiledBlock();

                functionBuilder.BuildInfo(engine, compiledBlock, ((SimpleRuleSetFunction)functionBuilder).Info);

                return compiledBlock;    
            }

            throw new InvalidOperationException("Unknown code construct.");
        }
    }
}
