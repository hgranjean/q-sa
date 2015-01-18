using Atum.Database.Surveillance.Models;
using Atum.Domain.Common;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;

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
            StandardDocument standardDocument = null;
            
            
            standardDocument = new StandardDocument();

            //using (var ctx = )
            //{

            //}
            
            
            
            
            string documentTitle = "Document Title";
            standardDocument.Title = documentTitle;
            

            //Add Chapters
            AddChapters(standardDocument);

            Assert.IsTrue(standardDocument.Chapters.Count > 0);
            Assert.IsTrue(standardDocument.Title.Length > 0);
            //Assert.IsTrue(standardDocument.SubscriberId.Length>0);
            Assert.IsTrue(standardDocument.OwnerId > -1);
            
            using (var ctx = new AtumSurveillanceContext())
            {
                ctx.StandardDocuments.Add(standardDocument);
                ctx.SaveChanges();
            }

            
            //string standardTitle = "Standard Title";
            //string standardKey = "Standard Key";

            //Standard standard = chapter.AddStandard(standardKey, standardTitle);


            //standard.TableOfContents = TOC;
            //string itemKey = "Item Key";
            //string itemTitle = "Item Title";
            //PerformanceItem performanceItem = standard.AddPerformanceItem(itemKey,itemTitle);
            
        }

        private void AddChapters(StandardDocument standardDocument)
        {
            //string[] chapterKeys = { "EC", "LS" };
            string[] chapterKeys = { "EC"};
            foreach (var item in chapterKeys)
            {
                Chapter chapter = LoadChapter(item.ToString(), standardDocument);
            }
        }

        internal Chapter LoadChapter(string chapterId, StandardDocument standardDocument)
        {
            XmlDocument xmlDoc = LoadChapterDoc(chapterId);
            string chapterTitlePath = "chapter/chaptertitle";
            string chapterKey = (chapterId.Length > 0 ? chapterId : "EC");
            string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
            Chapter chapter = standardDocument.AddChapter(chapterKey, chapterTitle);
            chapter.Title = chapterTitle;
            //chapter.Standards = 
                LoadElements(xmlDoc, chapter);

            return chapter;
        }

        private XmlDocument LoadChapterDoc(string chapterId)
        {
            var chapterFileName  = @"C:\Atum Technology Group\Projects\AtumRules\Dev1.01\rulesdev\SurveyWeb\Store\JointCommissionStandards\";

            if (chapterId.Length > 2)
            {
                chapterId = chapterId.Remove(2, chapterId.Length - 2);
            }
            
            chapterFileName = Path.Combine(chapterFileName, "EC_out.xml".Replace("EC", chapterId));

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(chapterFileName);

            return xmlDoc;
        }
        private static DocumentElements LoadElements(XmlDocument xmlDoc, Chapter chapter)
        {
            string elementsTitlePath = "chapter/titles[title]/*";
            string catIdPath = "epid";
            DocumentElements retVal = new DocumentElements();

            XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

            foreach (XmlNode node in nodes)
            {
                XmlAttribute att = node.Attributes[catIdPath];
                string itemKey = att.InnerText;

                Standard epCat = chapter.AddStandard(itemKey, node.InnerText);

                epCat.Title = node.InnerText;
                epCat.Key = itemKey;
                epCat.PerformanceItems = LoadEPItems(xmlDoc, epCat);

                //retVal.Add(epCat);
            }

            //return retVal;
            return chapter.Standards;
        }

        private static DocumentElements LoadEPItems(XmlDocument xmlDoc, Standard standard)
        {
            DocumentElements retVal = new DocumentElements();

            string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standard.Key);

            string epIdPath = "id";

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                //PerformanceItem epItem = new PerformanceItem();
                XmlAttribute att = node.Attributes[epIdPath];

                PerformanceItem epItem = standard.AddPerformanceItem(att.InnerText, "");
                epItem.Text = node.InnerText;
                epItem.EPId = int.Parse(att.InnerText);

                epItem.Notes = (ItemNotes)LoadNotes(xmlDoc, epItem, standard.Key);
                retVal.Add(epItem);
            }

            return retVal;
        }

        private static PerformanceItem LoadEPItem(XmlDocument xmlDoc, string standardId, string epId)
        {
            string itemPath = "chapter/elements/element[@epid='standardId' and @id='epId']".Replace("standardId", standardId).Replace("epId", epId);
            string epIdPath = "id";


            XmlNode node = xmlDoc.SelectSingleNode(itemPath);

            XmlAttribute att = node.Attributes[epIdPath];
            PerformanceItem retVal = new PerformanceItem(att.InnerText, "");
            retVal.Text = node.InnerText;
            retVal.EPId = int.Parse(att.InnerText);
            retVal.Notes = (ItemNotes)LoadNotes(xmlDoc, retVal, standardId);

            return retVal;
        }

        private static ItemNotes LoadNotes(XmlDocument xmlDoc, PerformanceItem epItem, string standardId)
        {
            ItemNotes retVal = new ItemNotes();

            string itemsPath = "chapter/notes/note[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardId).Replace("epItemId", epItem.EPId.ToString());

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                string note = node.InnerText;

                retVal.Add(new ItemNote(note));
            }

            return retVal;
        }


    }
}
