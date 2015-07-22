using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Atum.Repository.Surveillance;
using Atum.Database.Surveillance.Models;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Atum.Repository.Test
{
    [TestClass]
    public class StandardDocumentRepositoryTest
    {
        private IRepository<StandardDocument> _standardDocumentRepository;
        private DbContext _ctx = new AtumSurveillanceContext();

        [TestInitialize]
        public void TestInitialize()
        {
            _ctx = new AtumSurveillanceContext();
            _standardDocumentRepository = new StandardDocumentRepository(_ctx);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _standardDocumentRepository = null;
            _ctx = null;
        }

        //StandarDocument Root Aggregate
        [TestMethod]
        public void TestMethodAddStandardDocument()
        {
            //
            var standardDocument = new StandardDocument { Title = "test_standardDocument" };
            _standardDocumentRepository.Add(standardDocument);

            Assert.IsTrue(standardDocument.Id > 0);
        }

        //Guidelines/Documents
        [TestMethod]
        public void TestMethodFindStandardDocument()
        {
            int Id = 2;
            var documentById = _standardDocumentRepository.FindById(Id);

            Assert.IsNotNull(documentById);

            Guid guid = new Guid();
            var documentByGuid = _standardDocumentRepository.FindByGuid(guid);

            Assert.IsNotNull(documentByGuid);
        }

        [TestMethod]
        public void TestMethodFindMatchingStandardDocument()
        {
            //Expression criteria = new Expression<this.ab,bool>();

            //_standardDocumentRepository.FindMatching(criteria);           
        }

        [TestMethod]
        public void TestMethodDeleteStandardDocument()
        {
            //Find and Delete
            StandardDocument documentToDelete = _standardDocumentRepository.FindByGuid(new Guid());
            _standardDocumentRepository.Delete(documentToDelete);

        }


        //Chapters
        [TestMethod]
        public void TestMethodAddChapter()
        {
            //Assert.IsTrue(chapter.Id > 0);
        }

        [TestMethod]
        public void TestMethodFindChapter()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodFindMatchingChapter()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodDeleteChapter()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        //Standards
        [TestMethod]
        public void TestMethodAddStandard()
        {
            //Assert.IsTrue(chapter.Id > 0);
        }

        [TestMethod]
        public void TestMethodFindStandard()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodFindMatchingStandard()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodDeleteStandard()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        //PerformanceItems
        [TestMethod]
        public void TestMethodAddPerformanceItem()
        {
            //Assert.IsTrue(chapter.Id > 0);
        }

        [TestMethod]
        public void TestMethodFindPerformanceItem()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodFindMatchingPerformanceItem()
        {
            //Assert.IsNotNull(chapterByGuid);
        }

        [TestMethod]
        public void TestMethodDeletePerformanceItem()
        {
            //Assert.IsNotNull(chapterByGuid);
        }


        //private void AddChapters(StandardDocument standardDocument)
        //{
        //    //string[] chapterKeys = { "EC", "LS" };
        //    string[] chapterKeys = { "EC" };
        //    foreach (var item in chapterKeys)
        //    {
        //        Chapter chapter = LoadChapter(item.ToString(), standardDocument);
        //    }
        //}

        //internal Chapter LoadChapter(string chapterId, StandardDocument standardDocument)
        //{
        //    XmlDocument xmlDoc = LoadChapterDoc(chapterId);
        //    string chapterTitlePath = "chapter/chaptertitle";
        //    string chapterKey = (chapterId.Length > 0 ? chapterId : "EC");
        //    string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
        //    Chapter chapter = standardDocument.AddChapter(chapterKey, chapterTitle);
        //    chapter.Title = chapterTitle;
        //    //chapter.Standards = 
        //    LoadElements(xmlDoc, chapter);

        //    return chapter;
        //}

        //private XmlDocument LoadChapterDoc(string chapterId)
        //{
        //    var chapterFileName = @"C:\Atum Technology Group\Projects\AtumRules\Dev1.01\rulesdev\SurveyWeb\Store\JointCommissionStandards\";

        //    if (chapterId.Length > 2)
        //    {
        //        chapterId = chapterId.Remove(2, chapterId.Length - 2);
        //    }

        //    chapterFileName = Path.Combine(chapterFileName, "EC_out.xml".Replace("EC", chapterId));

        //    XmlDocument xmlDoc = new XmlDocument();

        //    xmlDoc.Load(chapterFileName);

        //    return xmlDoc;
        //}
        //private static DocumentElements LoadElements(XmlDocument xmlDoc, Chapter chapter)
        //{
        //    string elementsTitlePath = "chapter/titles[title]/*";
        //    string catIdPath = "epid";
        //    DocumentElements retVal = new DocumentElements();

        //    XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

        //    foreach (XmlNode node in nodes)
        //    {
        //        XmlAttribute att = node.Attributes[catIdPath];
        //        string itemKey = att.InnerText;

        //        Standard epCat = chapter.AddStandard(itemKey, node.InnerText);

        //        epCat.Title = node.InnerText;
        //        epCat.Key = itemKey;
        //        epCat.PerformanceItems = LoadEPItems(xmlDoc, epCat);

        //        //retVal.Add(epCat);
        //    }

        //    //return retVal;
        //    return chapter.Standards;
        //}

        //private static DocumentElements LoadEPItems(XmlDocument xmlDoc, Standard standard)
        //{
        //    DocumentElements retVal = new DocumentElements();

        //    string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standard.Key);

        //    string epIdPath = "id";

        //    XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

        //    foreach (XmlNode node in nodes)
        //    {
        //        //PerformanceItem epItem = new PerformanceItem();
        //        XmlAttribute att = node.Attributes[epIdPath];

        //        PerformanceItem epItem = standard.AddPerformanceItem(att.InnerText, "");
        //        epItem.Text = node.InnerText;
        //        epItem.EPId = int.Parse(att.InnerText);

        //        epItem.Notes = (ItemNotes)LoadNotes(xmlDoc, epItem, standard.Key);
        //        retVal.Add(epItem);
        //    }

        //    return retVal;
        //}

        //private static PerformanceItem LoadEPItem(XmlDocument xmlDoc, string standardId, string epId)
        //{
        //    string itemPath = "chapter/elements/element[@epid='standardId' and @id='epId']".Replace("standardId", standardId).Replace("epId", epId);
        //    string epIdPath = "id";


        //    XmlNode node = xmlDoc.SelectSingleNode(itemPath);

        //    XmlAttribute att = node.Attributes[epIdPath];
        //    PerformanceItem retVal = new PerformanceItem(att.InnerText, "");
        //    retVal.Text = node.InnerText;
        //    retVal.EPId = int.Parse(att.InnerText);
        //    retVal.Notes = (ItemNotes)LoadNotes(xmlDoc, retVal, standardId);

        //    return retVal;
        //}

        //private static ItemNotes LoadNotes(XmlDocument xmlDoc, PerformanceItem epItem, string standardId)
        //{
        //    ItemNotes retVal = new ItemNotes();

        //    string itemsPath = "chapter/notes/note[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardId).Replace("epItemId", epItem.EPId.ToString());

        //    XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

        //    foreach (XmlNode node in nodes)
        //    {
        //        string note = node.InnerText;

        //        retVal.Add(new ItemNote(note));
        //    }

        //    return retVal;
        //}


    }
}
