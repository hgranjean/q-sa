using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain;
using Rules.Engine.Base;
using Rules.Engine.Functions;
using Rules.Engine.Runtime;
using Rules.Engine.Session;

namespace Rules.Engine
{
    public class RuleSession : IDisposable
    {
        private Engine _engine;
        private bool _compiled = false;
        private readonly WorkingMemory _workingMemory;
        private readonly RuleApplicationInfo _ruleApplicationInfo;
        
        public RuleSession(RuleApplicationSpec ruleApplicationSpec)
        {
            _workingMemory = new WorkingMemory();
            _ruleApplicationInfo = new RuleApplicationInfo(ruleApplicationSpec);
        }

        public RuleSession(RuleApplicationReference ruleApplicationReference) :
            this(ruleApplicationReference.RuleApplicationSpec)
        {
        }

        public Entity CreateEntity(String name, Object boundValue = null)
        {
            if (_compiled == false)
            {
                Compile();
            }

            EntityInfo entityInfo = _ruleApplicationInfo.EntityInfos.FirstOrDefault(e => String.Compare(e.EntitySpec.Name, name, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (entityInfo == null)
            {
                throw new Exception("EntityInfo not found - " + name);
            }

            var containerId = new RuntimeIdentifier();

            var stateContainer = new StateContainer(this, entityInfo, containerId, boundValue);

            var entity = new Entity(this, stateContainer) { Id = containerId };

            _workingMemory.Set(name + "::" + entity.Id, entity);

            return entity;
        }

        public ExecutionResult ExecuteRules()
        {   
            if (_compiled == false)
            {
                Compile();
            }

            var execResult = new ExecutionResult();

            _ruleApplicationInfo.Execute(_engine, _workingMemory, execResult);

            return execResult;
        }

        private void Compile()
        {
            var builder = new FunctionInfo();

            _engine = new Engine(_ruleApplicationInfo);
            
            _ruleApplicationInfo.Compile(_engine, builder);

            _compiled = true;
        }

        public void Dispose()
        {   
            _workingMemory.Values.Clear();
        }
    }
}
