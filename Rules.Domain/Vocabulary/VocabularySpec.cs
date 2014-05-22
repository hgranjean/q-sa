using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rules.Domain.Vocabulary
{
    public class VocabularySpec
    {
        public List<TemplateSpec> Templates { get; set; }

        public VocabularySpec()
        {
            Templates = new List<TemplateSpec>();
        }
    }
}
