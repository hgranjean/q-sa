using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace SurveyWeb.Services
{
    internal class StandardsManagementServices
    {
        internal static StandardDocumentViewModel GetChapter(string chapterId)
        {

            StandardDocumentViewModel retVal = new StandardDocumentViewModel();
            
            //string chapterFileName = @"C:\Atum Technology Group\Rules Venture\Reference Docs\Joint Commision Standards\EC_out.xml";
            //XmlDocument xmlDoc = new XmlDocument();

            //xmlDoc.Load(chapterFileName);

            //string chapterTitlePath = "chapter/chaptertitle";
            Chapter chapter = loadChapter(chapterId);
            //string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
            //chapter.Title = chapterTitle;
            //chapter.Elements = loadElements(xmlDoc);

            retVal = buildStandardViewModel(chapter);
            

            return retVal;
        }

        internal static Chapter loadChapter(string chapterId)
        {
            //string chapterFileName = @"C:\Atum Technology Group\Rules Venture\Reference Docs\Joint Commision Standards\EC_out.xml";
            XmlDocument xmlDoc = loadChapterDoc();// new XmlDocument();

            //xmlDoc.Load(chapterFileName);

            string chapterTitlePath = "chapter/chaptertitle";
            Chapter chapter = new Chapter();
            string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
            chapter.Title = chapterTitle;
            chapter.Elements = loadElements(xmlDoc);
            
            return chapter;
        }

        private static XmlDocument loadChapterDoc()
        {

            string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/");
            
            string chapterFileName = appPath + "EC_out.xml";

            XmlDocument xmlDoc =  new XmlDocument();
            
            xmlDoc.Load(chapterFileName);
            return xmlDoc;
        }

        private static StandardDocumentViewModel buildStandardViewModel(Chapter chapter)
        {
            StandardDocumentViewModel retVal = new StandardDocumentViewModel();

            retVal.Title = chapter.Title;
            retVal.TableOfContents = buildTOC(chapter.Elements);

            return retVal;
        }
        
        private static List<TOCElementViewModel> buildTOC(List<Standard> list)
        {
            List<TOCElementViewModel> retVal = new List<TOCElementViewModel>();
            
            foreach (var item in list)
            {
                TOCElementViewModel toc = new TOCElementViewModel();

                toc.Key = item.StandardId;
                toc.Title = item.Title;
                toc.Content = item.StandardId;
                retVal.Add(toc);
            }

            return retVal;

        }

        private static List<Standard> loadElements(XmlDocument xmlDoc)
        {
            string elementsTitlePath = "chapter/titles[title]/*";
            string catIdPath = "epid";
            List<Standard> retVal = new List<Standard>();

            XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

            foreach (XmlNode node in nodes)
            {
                Standard epCat = new Standard();
                XmlAttribute att = node.Attributes[catIdPath];

                epCat.Title = node.InnerText;
                epCat.StandardId = att.InnerText;
                epCat.Items = LoadEPItems(xmlDoc, epCat.StandardId);

                retVal.Add(epCat);
                //if (!categoryLookup.ContainsKey(epCat.StandardId))
                //{
                //    categoryLookup.Add(epCat.StandardId, epCat);
                //}
            }

            return retVal;
        }

        private static List<ElementOfPerformance> LoadEPItems(XmlDocument xmlDoc, string standardId)
        {
            List<ElementOfPerformance> retVal = new List<ElementOfPerformance>();

            string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standardId);

            string epIdPath = "id";

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                ElementOfPerformance epItem = new ElementOfPerformance();
                XmlAttribute att = node.Attributes[epIdPath];

                epItem.Text = node.InnerText;
                epItem.EPId = int.Parse(att.InnerText);
                epItem.Notes = LoadNotes(xmlDoc, epItem, standardId);
                retVal.Add(epItem);
            }

            return retVal;
        }

        private static ElementOfPerformance LoadEPItem(XmlDocument xmlDoc, string standardId, string epId)
        {
            ElementOfPerformance  retVal = new ElementOfPerformance();
            //element epid='EC.01.01.01' id='1'
            string itemPath = "chapter/elements/element[@epid='standardId' and @id='epId']".Replace("standardId", standardId).Replace("epId", epId);
            string epIdPath = "id";


            XmlNode node = xmlDoc.SelectSingleNode(itemPath);
                
            XmlAttribute att = node.Attributes[epIdPath];
            retVal.Text = node.InnerText;
            retVal.EPId = int.Parse(att.InnerText);
            retVal.Notes = LoadNotes(xmlDoc, retVal, standardId);

            return retVal;
        }
        private static List<string> LoadNotes(XmlDocument xmlDoc, ElementOfPerformance epItem, string standardId)
        {
            List<string> retVal = new List<string>();

            string itemsPath = "chapter/notes/note[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardId).Replace("epItemId", epItem.EPId.ToString());

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                string note = node.InnerText;

                retVal.Add(note);
            }

            return retVal;
        }


        internal static TOCElementViewModel GetStandardElement(string standardElementId)
        {
            TOCElementViewModel retVal = new TOCElementViewModel();
            string chapterId = GetChapterId(standardElementId);
            Chapter chapter = loadChapter(chapterId);


            Standard ep = chapter.GetPerformanceCategory(standardElementId);
            retVal.Title = ep.Title;
            retVal.Elements = loadTOCElements(ep);
            return retVal;

        }

        private static List<TOCElementViewModel> loadTOCElements(Standard ep)
        {
            List<TOCElementViewModel> retVal = new List<TOCElementViewModel>();
            List<ElementOfPerformance> list = ep.Items;

            foreach (var item in list)
            {
                TOCElementViewModel toc = new TOCElementViewModel();

                toc.Key = item.EPId.ToString();
                toc.Content = item.Text;
                toc.ParentKey = ep.StandardId;
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
        internal static PerformanceElementViewModel GetPerformanceElementViewModel(string standardElementId, string performanceItemId)
        {
            XmlDocument xmlDoc = loadChapterDoc();// new XmlDocument();
            PerformanceElementViewModel retVal = new PerformanceElementViewModel();
            //string itemsPath = "chapter/notes/note[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardId).Replace("epItemId", epItem.EPId.ToString());

            string performanceNodePath = "chapter/referencedelements/referencedelement[@epid='standardId' and @itemid='epItemId']".Replace("standardId", standardElementId).Replace("epItemId", performanceItemId);

            XmlNode performanceItemNode = xmlDoc.SelectSingleNode(performanceNodePath);

            retVal.EPId = performanceItemId;
            if (performanceItemNode!=null)
            {
                retVal.Content = performanceItemNode.InnerText;
                
            }

            ElementOfPerformance epItem = new ElementOfPerformance();
            epItem.EPId = int.Parse(performanceItemId);

            retVal.Notes = LoadNotes(xmlDoc, epItem, standardElementId);
            retVal.ReferencedElementLinks = setReferencedElementLinks(epItem, standardElementId, xmlDoc);

            return retVal;
        }

        private static List<HtmlString> setReferencedElementLinks(ElementOfPerformance epItem, string standardId,XmlDocument xmlDoc)
        {
            //<referencedelement itemid='4' epid='EC.01.01.01'><element>EC.04.01.01</element><epitem>EP 15</epitem></referencedelement>
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
    }
}