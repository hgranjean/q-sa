using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Session;

namespace Rules.Engine.Runtime
{
    public class Entity : RuntimeObject
    {
        private RuleSession _session;
        private StateContainer _stateContainer;
        
        internal Entity(RuleSession session, StateContainer stateContainer)
        {
            _session = session;
            _stateContainer = stateContainer;
        }

        internal StateContainer StateContainer { get { return _stateContainer; } }
    }
}
