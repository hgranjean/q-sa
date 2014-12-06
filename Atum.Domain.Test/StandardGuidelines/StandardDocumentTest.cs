using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Atum.Domain.Common;

namespace Atum.Domain.Test.StandardGuidelines
{
    [TestClass]
    public class StandardDocumentTest
    {
        [TestMethod]
        public void TestGuidelines()
        {
            //An internal guideline belongs to a particular Health System
            /*
                Standards and Guidelines (aka Documents)- Documents containing a grouped collection of unique requirements for compliance 
                Guideline: Owner= (Provided by AQS) Joint Commission/NCQA/External or (Provided by Subscribing Health System)Internal -> 
             *      A collection of Chapters in the case of The Joint Commission Standards
                Document (e.g. Joint Commission Chapter): A collection of Performance Elements
                Performance Element (e.g. a Joint Commission Standard): A collection of Performance Items
                PerformanceItem: (e.g. Element of Performance)             
             */
               
            //Create Guideline
            //Guideline guideline = new Guideline("Proposed Core Reqirements - All chapters Hospital Accreditation Program");
            
            ////Document (e.g. Joint Commission Chapter): A collection of Performance Elements
            //Document document = new Document("Environment of Care Chapter");
           
            ////Performance Element (e.g. a Joint Commission Standard): A collection of Performance Items
            //string elementKey = "EC.01.01.01";
            //string elementTitle = "EC.01.01.01 - The organization plans activities that minimize risks in the environment of care.";
            ////Note: One or more persons can be assigned to 
            //Element element = new Element(elementKey,elementTitle);
            //element.Content = "The organization plans activities that minimize risks in the environment of care. Note: One or more persons can be assigned to manage risks associated with the management plans described in this standard.";

            ////Performance Item: (e.g. Element of Performance)
            //string itemKey = "EP 1";
            //string itemTitle = "EC.01.01.01 - The organization plans activities that minimize risks in the environment of care. Note: One or more persons can be assigned to ";
            //Item item = new Item(itemKey,itemTitle);
            //item.Text = "";
            //item.Notes.Add("This is a note!");

        }

        [TestMethod]
        public void TestStandardDocument()
        {
            StandardDocument standardDocument = new StandardDocument();
            string documentTitle = "Document Title";
            standardDocument.Title = documentTitle;
            standardDocument.Chapters = new System.Collections.Generic.List<Chapter>();

            string chapterTitle = "Chapter Title";
            string chapterKey = "Chapter Key";
            Chapter chapter = standardDocument.AddChapter(chapterKey, chapterTitle);
            
            string standardTitle = "Standard Title";
            string standardKey = "Standard Key";

            Standard standard = chapter.AddStandard(standardKey, standardTitle);


            //standard.TableOfContents = TOC;
            string itemKey = "Item Key";
            string itemTitle = "Item Title";
            PerformanceItem performanceItem = standard.AddPerformanceItem(itemKey,itemTitle);
            
            
        }

        
    }
}
