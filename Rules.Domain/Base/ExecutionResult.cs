using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain
{
    [Serializable]
    public class ExecutionResult
    {
        public List<Object> ReturnValues = new List<Object>();

        public ExecutionResult()
        {
            
        }

        public ExecutionResult(Object returnValue)
        {
            ReturnValues.Add(returnValue);
        }

        public void Add(Object returnValue)
        {
            ReturnValues.Add(returnValue);
        }
    }
}
