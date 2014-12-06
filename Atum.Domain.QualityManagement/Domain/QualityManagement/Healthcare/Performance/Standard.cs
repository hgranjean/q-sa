using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    /// <summary>
    /// 
    /// </summary>
    public class Standard : TOCElement
    {
        Dictionary<string, PerformanceItem> _luPerformanceItems = new Dictionary<string, PerformanceItem>();
        public StringBuilder _body = new StringBuilder();

        public Standard()//:base("","") 
        {
            this.TableOfContents = new TableOfContents();
        }

        public Standard(string key, string title)//:base(key,title)
        {
            this.TableOfContents = new TableOfContents();
        }
        public string StandardId { get; set; }
        public string Title { get; set; }

        public List<PerformanceItem> PerformanceItems { get; set; }

        public StringBuilder Body { get; private set; }

        public string Observation { get; set; }

        public Common.TableOfContents TableOfContents { get; set; }

        public void AddBodyText(string text)
        {
            Body.Append(text);
        }
        public PerformanceItem AddPerformanceItem(string itemKey, string itemTitle)
        {
            //Check Key
            if (_luPerformanceItems.Keys.Contains(itemKey))
            {
                throw new Exception("Key Exists");
            }
            else //Add to PerformanceItem Lookup
            {
                PerformanceItem performanceItem = new PerformanceItem(itemKey, itemTitle);
                PerformanceItems.Add(performanceItem);
                _luPerformanceItems.Add(itemKey, performanceItem);
                //Update TOC
                TableOfContents.AddElement(performanceItem);
                return performanceItem;
            }
        }
    }
}
