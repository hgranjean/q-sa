using Atum.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Healthcare.Performance
{
    public class xDocument
    {
        public xDocument(string title)
        {
            // TODO: Complete member initialization
            this.Title = title;
        }
        public string Title { get; set; }
        

        public TableOfContents TOC { get; set; }


        public DocumentElement AddElement(DocumentElement element) 
        {
            //TODO: Maintain Table of Contents Here
            //Elements.Add(element);
            TOC.AddElement(element);
            return element;
        } 
        
        
        //public Element GetPerformanceCategory(string standardElementId)
        //{
        //    int length = Elements.Count;

        //    for (int i = 0; i < length; i++)
        //    {
        //        if (Elements[i].Key.Equals(standardElementId))
        //        {
        //            return Elements[i];
        //        }
        //    };

        //    return null;
        //}
    }
}
