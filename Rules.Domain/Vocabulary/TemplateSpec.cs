using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rules.Domain.Vocabulary.Templates;

namespace Rules.Domain.Vocabulary
{
    public class TemplateSpec : RuleObjectBase
    {
        public enum TemplateType
        {
            Format, Bullet
        };

        public String DisplayText { get; set; }
        public string FunctionName { get; set; }
        public String Prototype { get; set; }
        public String Expression { get; set; }
        public List<Placeholder> Placeholders { get; set; }
        public TemplateType Type { get; set; }
        public TemplateCategory Category { get; set; }
        public TemplateDataType DataType { get; set; }
    }
}
