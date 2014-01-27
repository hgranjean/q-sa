using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Base;

namespace Rules.Engine.Session
{
    internal class StateContainer
    {
        private RuleSession _session;
        private EntityInfo _containerInfo;
        private RuntimeIdentifier _containerId;
        private Object _boundValue;

        public StateContainer(RuleSession session, EntityInfo containerInfo, RuntimeIdentifier containerId, Object boundValue)
        {
            _session = session;
            _containerInfo = containerInfo;
            _containerId = containerId;
            _boundValue = boundValue;
        }

        internal EntityInfo ContainerInfo { get { return _containerInfo; } }

        public void Execute(WorkingMemory workingMemory)
        {
            _containerInfo.Execute(this, workingMemory);
        }

        public Object this[String propertyName]
        {
            get { return _containerInfo.EntitySpec.BoundType.GetProperty(propertyName).GetValue(_boundValue); }
            set { _containerInfo.EntitySpec.BoundType.GetProperty(propertyName).SetValue(_boundValue, value); }
        }
    }
}
