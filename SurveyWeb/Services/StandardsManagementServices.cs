using Atum.Domain.Common;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Atum.Utility.XML;
using SurveyWeb.Models.StandardMaintenance;
using SurveyWeb.Repository;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Xml;

namespace SurveyWeb.Services
{
    /// <summary>
    /// TODO: Replace XmlDocument with XDocument!
    /// </summary>
    public class StandardsManagementServices
    {
        private readonly ISurveyStore _store;

        public StandardsManagementServices(ISurveyStore store)
        {
            _store = store;
        }

        internal StandardDocumentViewModel GetChapter(string chapterId)
        {
            StandardDocumentViewModel retVal = new StandardDocumentViewModel();
            
            Chapter chapter = LoadChapter(chapterId);

            retVal = BuildStandardViewModel(chapter);

            return retVal;
        }

        internal Chapter LoadChapter(string chapterId)
        {
            XmlDocument xmlDoc = LoadChapterDoc(chapterId);

            string chapterTitlePath = "chapter/chaptertitle";
            //Chapter chapter = new Chapter();
            string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
            Chapter chapter = new Chapter(chapterId, chapterTitle);
            chapter.Title = chapterTitle;
            chapter.Standards = LoadElements(xmlDoc);
            
            return chapter;
        }

        private XmlDocument LoadChapterDoc(string chapterId)
        {
            if (chapterId.Length>2)
            {
                chapterId = chapterId.Remove(2, chapterId.Length - 2);  
            }
            var chapterFileName = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "EC_out.xml".Replace("EC",chapterId));

            XmlDocument xmlDoc =  new XmlDocument();
            
            xmlDoc.Load(chapterFileName);

            return xmlDoc;
        }

        private StandardDocumentViewModel BuildStandardViewModel(Chapter chapter)
        {
            StandardDocumentViewModel retVal = new StandardDocumentViewModel();

            retVal.Title = chapter.Title;
            //retVal.TableOfContents = BuildTOC(chapter.Elements);

            return retVal;
        }
        
        private static List<TOCElementViewModel> BuildTOC(List<Standard> list)
        {
            List<TOCElementViewModel> retVal = new List<TOCElementViewModel>();
            
            foreach (var item in list)
            {
                TOCElementViewModel toc = new TOCElementViewModel();

                toc.Key = item.Key;
                toc.Title = item.Title;
                toc.Content = item.Key;
                retVal.Add(toc);
            }

            return retVal;

        }

        private static DocumentElements LoadElements(XmlDocument xmlDoc)
        {
            string elementsTitlePath = "chapter/titles[title]/*";
            string catIdPath = "epid";
            DocumentElements retVal = new DocumentElements();

            XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

            foreach (XmlNode node in nodes)
            {
                XmlAttribute att = node.Attributes[catIdPath];
                string itemKey = att.InnerText;

                Standard epCat = new Standard(itemKey,node.InnerText);

                epCat.Title = node.InnerText;
                epCat.Key= itemKey;
                epCat.PerformanceItems = LoadEPItems(xmlDoc, epCat.Key);

                retVal.Add(epCat);
            }

            return retVal;
        }

        private static DocumentElements LoadEPItems(XmlDocument xmlDoc, string standardId)
        {
            DocumentElements retVal = new DocumentElements();

            string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standardId);

            string epIdPath = "id";

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                //PerformanceItem epItem = new PerformanceItem();
                XmlAttribute att = node.Attributes[epIdPath];

                PerformanceItem epItem = new PerformanceItem(att.InnerText,"");
                epItem.Text = node.InnerText;
                epItem.EPId = int.Parse(att.InnerText);
             
                epItem.Notes = LoadNotes(xmlDoc, epItem, standardId);
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
            PerformanceItem retVal = new PerformanceItem(att.InnerText,"");
            retVal.Text = node.InnerText;
            retVal.EPId = int.Parse(att.InnerText);
            retVal.Notes = LoadNotes(xmlDoc, retVal, standardId);

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


        internal TOCElementViewModel GetStandardElement(string standardElementId)
        {
            TOCElementViewModel retVal = new TOCElementViewModel();
            string chapterId = GetChapterId(standardElementId);
            Chapter chapter = LoadChapter(chapterId);


            Standard ep = chapter.GetPerformanceCategory(standardElementId);
            retVal.Title = ep.Title;
            retVal.Elements = LoadTOCElements(ep);
            return retVal;

        }

        private static List<TOCElementViewModel> LoadTOCElements(Standard standard)
        {
            List<TOCElementViewModel> retVal = new List<TOCElementViewModel>();
            DocumentElements list = standard.PerformanceItems;

            foreach (PerformanceItem item in list)
            {
                TOCElementViewModel toc = new TOCElementViewModel();

                toc.Key = item.EPId.ToString();
                toc.Content = item.Text;
                toc.ParentKey = standard.Key;
                retVal.Add(toc);
            }

            return retVal;
        }

        private static string GetChapterId(string standardElementId)
        {
            string retVal = "";
            try
            {
                retVal = standardElementId.Split('.')[0];
            }
            catch 
            {
            }

            return retVal;
        }
        /// <summary>
        /// Performance Element View Model
        /// </summary>
        /// <param name="standardElementId"></param>
        /// <param name="performanceItemId"></param>
        /// <returns></returns>
        internal PerformanceElementViewModel GetPerformanceElementViewModel(string chapterId, string standardElementId, string performanceItemId)
        {
            XmlDocument xmlDoc = LoadChapterDoc(chapterId);// new XmlDocument();
            PerformanceElementViewModel retVal = new PerformanceElementViewModel();

            string performanceNodePath = "chapter/referencedelements/referencedelement[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardElementId).Replace("epItemId", performanceItemId);

            XmlNode performanceItemNode = xmlDoc.SelectSingleNode(performanceNodePath);

            retVal.EPId = performanceItemId;
            if (performanceItemNode!=null)
            {
                retVal.Content = performanceItemNode.InnerText;
                
            }

            PerformanceItem epItem = new PerformanceItem("","");
            epItem.EPId = int.Parse(performanceItemId);

            retVal.Notes = LoadNotes(xmlDoc, epItem, standardElementId).GetNotes();
            retVal.ReferencedElementLinks = setReferencedElementLinks(epItem, standardElementId, xmlDoc);

            return retVal;
        }

        private static List<HtmlString> setReferencedElementLinks(PerformanceItem epItem, string standardId, XmlDocument xmlDoc)
        {
            List<HtmlString> retVal = new List<HtmlString>();
            string refElementPath = "chapter/referencedelements/referencedelement[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardId).Replace("epItemId", epItem.EPId.ToString());
            string refElementIdPath = "element";
            string refEPIdPath = "epitem";

            XmlNodeList xmlNodes = xmlDoc.SelectNodes(refElementPath);
            foreach (XmlNode nodeItem in xmlNodes)
            {
                HtmlString htmlString = new HtmlString("<span><a href=\"StandardElement?standardElementId=linkId\">linkId</a> - epId</span>".Replace("linkId",
                    nodeItem.SelectSingleNode(refElementIdPath).InnerText).Replace("epId",
                    nodeItem.SelectSingleNode(refEPIdPath).InnerText));

                retVal.Add(htmlString);
            }


            return retVal;
        }
        public IEnumerable<KeyValuePair<string, DocumentElement>> GetTOCs()
        {
            //yield return new KeyValuePair<string, TOCElement>(string.Empty, TOCElement.None);
            yield return new KeyValuePair<string, DocumentElement>("LS.02.01.20 EP27", GetViewModel("LS.02.01.20 EP27"));
            yield return new KeyValuePair<string, DocumentElement>("LS.04.03.02", GetViewModel("LS.04.03.02"));            
        }

        /// <summary>
        /// Standard Content
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        private DocumentElement GetViewModel(string Id)
        {
            DocumentElement model = new Chapter(Id, Id);

            if (Id == "LS.02.01.20 EP27")
            {
                //model.Content = LoadContent(Id);
            }

            if (Id == "LS.04.03.02")
            {
                var appPath = _store.GetPath(StoreType.Standards);
                
                model = (DocumentElement)XmlSerializationUtility.GetObjectFromFile(Path.Combine(appPath, Id + ".xml"), typeof(DocumentElement));
            }

            return model;
        }


        internal StandardDocumentViewModel LoadDocument(int? id)
        {
            //TODO: Load Document Title Form Store
            var retVal = new StandardDocumentViewModel { Title = "Proposed Core Reqirements - All chapters Hospital Accreditation Program" };
            retVal.TableOfContents = LoadTableOfContent();
            return retVal;
        }

        //TODO: Move to aoppropriate store
        private IEnumerable<TOCElementViewModel> LoadTableOfContent()
        {
            yield return new TOCElementViewModel { Key = "EC", Title = "Environment of Care (EC)" };
            yield return new TOCElementViewModel { Key = "EM", Title = "Emergency Management (EM)" };
            yield return new TOCElementViewModel { Key = "HR", Title = "Human Resources (HR) " };
            yield return new TOCElementViewModel { Key = "IC", Title = "Inmation Management (IM) " };
            yield return new TOCElementViewModel { Key = "LD", Title = "Leadershifection Prevention and Control (IC)" };
            yield return new TOCElementViewModel { Key = "IM", Title = "Inforp (LD) " };
            yield return new TOCElementViewModel { Key = "LS", Title = "Life Safety (LS)" };
            yield return new TOCElementViewModel { Key = "MM", Title = "Medication Managment (MM) " };
            yield return new TOCElementViewModel { Key = "PC", Title = "Provision of Care,e Treatment, and Services (PC) " };
            yield return new TOCElementViewModel { Key = "PC", Title = "Performance Improvement (PI)" };
            yield return new TOCElementViewModel { Key = "RC", Title = "Record of Care, Treatment, and Services (RC) " };
            yield return new TOCElementViewModel { Key = "RI", Title = "Rights and Responsibilities of the Individual (RI)" };
            yield return new TOCElementViewModel { Key = "WT", Title = "Waived Testing (WT)" };
        }


        internal IEnumerable<StandardDocumentViewModel> GetStandardDocuments()
        {
            yield return new StandardDocumentViewModel { Id = 1, Title = "Joint Commission Standards", Key = "TJC", Text = "Text: Document Description goes here!", Owner = (new Person("Zorina","","Granjean")), Visibility = "Public" };
            yield return new StandardDocumentViewModel { Id = 2, Title = "Hospital System Specific Guidelines", Key = "SysId:HSSG", Text = "Text: Document Description goes here!", Owner = (new Person("John", "D", "Quality-Manager")), Visibility = "Private" };
        }
    }
}