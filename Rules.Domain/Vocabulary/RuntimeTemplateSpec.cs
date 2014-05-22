using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain.Vocabulary
{
    public class RuntimeTemplateSpec : TemplateSpec
    {
        public List<RuntimePlaceholderValue> RuntimePlaceholder { get; set; }
    }
}
