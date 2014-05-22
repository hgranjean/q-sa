using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain.Vocabulary
{
    public class RuntimePlaceholderValue : Placeholder
    {
        private String value;
        private RuntimeTemplateSpec root;
    }
}
