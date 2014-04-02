using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rules.Engine.Infos;

namespace Rules.Engine.Functions.Builders
{
    internal class DataTypeInfo : IDataTypeInfo
    {
        private readonly Type _systemType;

        public DataTypeInfo(string dataType)
        {
            _systemType = Type.GetType(dataType);

            // If not found among the loaded types, probe for a system type by adding namespace.
            if (_systemType == null)
            {
                _systemType = Type.GetType(String.Concat("System", Type.Delimiter, dataType));
            }
        }

        public Type SystemType
        {
            get { return _systemType; }
        }
    }
}
