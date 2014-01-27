using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Engine
{
    internal class WorkingMemory
    {
        public Dictionary<Object, Object> Values { get; set; }

        public WorkingMemory()
        {
            Values = new Dictionary<object, object>();
        }

        public Object Get(Object key)
        {
            Object value = null;
            if (Values.TryGetValue(key, out value))
            {
                return value;
            }
            return null;
        }

        public void Set(Object key, Object value)
        {
            Object oldValue = null;
            if (Values.TryGetValue(key, out oldValue))
            {
                Values[key] = value;
            }
            else
            {
                Values.Add(key, value);   
            }
        }
    }
}
