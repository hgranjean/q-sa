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

        private static Chapter loadChapter(string chapterId)
        {
            string chapterFileName = @"C:\Atum Technology Group\Rules Venture\Reference Docs\Joint Commision Standards\EC_out.xml";
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(chapterFileName);

            string chapterTitlePath = "chapter/chaptertitle";
            Chapter chapter = new Chapter();
            string chapterTitle = xmlDoc.SelectSingleNode(chapterTitlePath).InnerText;
            chapter.Title = chapterTitle;
            chapter.Elements = loadElements(xmlDoc);
            
            return chapter;
        }

        private static StandardDocumentViewModel buildStandardViewModel(Chapter chapter)
        {
            StandardDocumentViewModel retVal = new StandardDocumentViewModel();

            retVal.Title = chapter.Title;
            retVal.TableOfContents = buildTOC(chapter.Elements);

            return retVal;
        }
        
        private static List<TOCElementViewModel> buildTOC(List<PerformanceCategory> list)
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

        private static List<PerformanceCategory> loadElements(XmlDocument xmlDoc)
        {
            string elementsTitlePath = "chapter/titles[title]/*";
            string catIdPath = "epid";
            List<PerformanceCategory> retVal = new List<PerformanceCategory>();

            XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

            foreach (XmlNode node in nodes)
            {
                PerformanceCategory epCat = new PerformanceCategory();
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

        private static List<Item> LoadEPItems(XmlDocument xmlDoc, string standardId)
        {
            List<Item> retVal = new List<Item>();

            string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standardId);

            string epIdPath = "id";

            XmlNodeList nodes = xmlDoc.SelectNodes(itemsPath);

            foreach (XmlNode node in nodes)
            {
                Item epItem = new Item();
                XmlAttribute att = node.Attributes[epIdPath];

                epItem.Text = node.InnerText;
                epItem.EPId = int.Parse(att.InnerText);
                epItem.Notes = LoadNotes(xmlDoc, epItem, standardId);
                retVal.Add(epItem);
            }

            return retVal;
        }

        private static List<string> LoadNotes(XmlDocument xmlDoc, Item epItem, string standardId)
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


            PerformanceCategory ep = chapter.GetPerformanceCategory(standardElementId);
            retVal.Title = ep.Title;
            retVal.Elements = loadTOCElements(ep);
            return retVal;

        }

        private static List<TOCElementViewModel> loadTOCElements(PerformanceCategory ep)
        {
            List<TOCElementViewModel> retVal = new List<TOCElementViewModel>();
            List<Item> list = ep.Items;

            foreach (var item in list)
            {
                TOCElementViewModel toc = new TOCElementViewModel();

                toc.Key = item.EPId.ToString();
                toc.Content = item.Text;
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
    }
}