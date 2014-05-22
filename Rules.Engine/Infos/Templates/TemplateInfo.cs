using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain.Vocabulary;
using Rules.Engine.Base;
using Rules.Engine.Functions;

namespace Rules.Engine.Infos.Templates
{
    internal class TemplateInfo : IInfo
    {
        public IInfo ValueInfo { get; set; }
        public IDataTypeInfo ValueType { get; set; }
        public String Name { get; set; }
        public CompileContext Context { get; set; }

        public TemplateSpec TemplateSpec { get; set; }
        public LambdaExpression Lambda { get; set; }
    }
}
