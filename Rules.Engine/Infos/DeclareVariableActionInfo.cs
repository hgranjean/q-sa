using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Engine.Functions;

namespace Rules.Engine.Infos
{
    internal class DeclareVariableActionInfo : IInfo
    {
        public IInfo ValueInfo { get; set; }
        public IDataTypeInfo ValueType { get; set; }
        public String VariableName { get; set; }
        public CompileContext Context { get; set; }
    }
}
