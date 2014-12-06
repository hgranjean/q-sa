using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class Chapter : TOCElement
    {
        Dictionary<string, Standard> _luStandards = new Dictionary<string, Standard>();

        public Chapter(string key, string title)//:base(key,  title)
        {
            this.TableOfContents = new TableOfContents();
        }

        public string Title { get; set; }

        public Standard GetPerformanceCategory(string standardElementId)
        {
            throw new NotImplementedException();
        }

        public TableOfContents TableOfContents { get; private set; }

        public List<Standard> Standards { get; set; }

        public Standard AddStandard(string standardKey, string standardTitle)
        {
            //Check Key
            if (_luStandards.Keys.Contains(standardKey))
            {
                throw new Exception("Key Exists");
            }
            else //Add to Standard Lookup
            {
                Standard standard = new Standard(standardKey, standardTitle);
                Standards.Add(standard);
                _luStandards.Add(standardKey, standard);
                //Update TOC
                TableOfContents.AddElement(standard);
                return standard;
            }
        }

    }
}
