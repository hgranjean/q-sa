using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Engine
{
    public class RuntimeIdentifier
    {
        private static int nextIdentifier = 1;

        public int? Value { get; private set; }

        public RuntimeIdentifier()
        {
            this.Value = nextIdentifier;
            
            nextIdentifier++;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
