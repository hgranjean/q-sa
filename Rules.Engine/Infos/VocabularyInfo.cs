using System.Collections.Generic;
using Rules.Engine.Base;
using Rules.Engine.Infos.Templates;

namespace Rules.Engine.Infos
{
    internal class VocabularyInfo : InfoBase
    {
        public List<TemplateInfo> TemplateInfos { get; set; }

        public VocabularyInfo()
        {
            TemplateInfos = new List<TemplateInfo>();
        }
    }
}
