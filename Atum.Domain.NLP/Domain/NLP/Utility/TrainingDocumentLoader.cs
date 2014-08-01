using Atum.Domain.NLP.NaiveBayes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Atum.Domain.NLP.Utility
{
    public class TrainingDocumentLoader
    {
        public static List<NaiveBayes.TrainingDocument> Load(string testDocument, Tokenizer tokenizer)
        {
            List<NaiveBayes.TrainingDocument> retVal = new List<TrainingDocument>();

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(testDocument);

            string documentsMatch = "Documents[DOC]/*";

            XmlNodeList docNodes = xmlDoc.SelectNodes(documentsMatch);
            foreach (XmlNode item in docNodes)
            {
                string docClass = item.SelectSingleNode("EP").InnerText;
                string classIdPattern = @"[A-Z]{2,4}.[0-9]{2}.[0-9]{2}.[0-9]{2}";
                XmlNode node = item.SelectSingleNode("FINDING");
                if (node != null)
                {
                    string docText = item.SelectSingleNode("FINDING").InnerText;

                    docClass = Regex.Match(docClass, classIdPattern).Value;

                    TrainingDocument classDoc = new TrainingDocument(docClass, docText, tokenizer);

                    retVal.Add(classDoc);
                }
            }
            return retVal;
        }



    }
}
