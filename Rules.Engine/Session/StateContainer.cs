using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Base;

namespace Rules.Engine.Session
{
    internal class StateContainer : DynamicObject, IDynamicMetaObjectProvider
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


        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var propertyName = binder.Name;

            if (_containerInfo.EntitySpec.BoundType.GetProperty(propertyName) != null)
            {
                result = _containerInfo.EntitySpec.BoundType.GetProperty(propertyName).GetValue(_boundValue);
                return true;
            }
            
            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var propertyName = binder.Name;

            if (_containerInfo.EntitySpec.BoundType.GetProperty(propertyName) != null)
            {
                _containerInfo.EntitySpec.BoundType.GetProperty(propertyName).SetValue(_boundValue, value);
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _containerInfo.EntitySpec.BoundType.GetProperties().Select(prop => prop.Name).ToList();
        }

        public DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new StateContainerDynamicMetaObject(parameter, 1);
        }

        /*public class StateContainerMetaObject : DynamicMetaObject
        {
            public StateContainerMetaObject()
        }*/
    }

    public class StateContainerBinder : DynamicMetaObjectBinder
    {
        public StateContainerBinder()
        {
            
        }
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            return new StateContainerDynamicMetaObject(Expression.Parameter(typeof(StateContainer), "Context"), null);
        }
    }

    public class StateContainerDynamicMetaObject : DynamicMetaObject
    {
           public StateContainerDynamicMetaObject(Expression expression, object value)
               : base(expression, BindingRestrictions.Empty, value)
           {
               ;
           }
    }
}
