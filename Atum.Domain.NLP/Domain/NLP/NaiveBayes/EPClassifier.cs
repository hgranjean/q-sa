using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Atum.Domain.NLP.Domain.NLP.NaiveBayes;
using Atum.Domain.NLP.Domain.NLP;
using System.IO;
using System.Xml.Serialization;

namespace Atum.Domain.NLP.NaiveBayes
{
    public class EPClassifier
    {

        public const string MATCH_FINDING_EP = "^<EP>[A-Z]{2,4}.[0-9]{2}.[0-9]{2}.[0-9]{2}";
        public const string MATCH_EP_FINDING = "^<EP></EP><finding>";
        public static int counter = 1;

        public EPClassifier(string modelPath)
        {
            this.Tokenizer = new Tokenizer(getExcludedWords(),modelPath);
            this.TrainingSet = new TrainingSet();
        }

        public static List<string> getExcludedWords()
        {
            string[] excludedWords = { "hospital", "the", "and", "a", "is", "are","s", "not","an","e","to","or","for","of", "in","out","other","were","was","be",
                                         "because","these","it","has","than","that","there","do","did","should","by","with","being"
                                     ,"as","had","no","on","if","from","into","those","been","could","would","have","ii","but","all","since","at","may","now","will","does","any"};

            return new List<string>(excludedWords);
        }


        private XmlNodeList LoadClassText(XmlDocument xmlDoc, string standardId)
        {
            string itemsPath = "chapter/elements/element[@epid='standardId']".Replace("standardId", standardId);
            return xmlDoc.SelectNodes(itemsPath);
        }
        /// <summary>
        /// Training From Standard
        /// </summary>
        /// <param name="chapterFile"></param>
        public void trainFromChapter(string chapterFile)
        {
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(chapterFile);

            string elementsTitlePath = "chapter/titles[title]/*";
            string catIdPath = "epid";

            XmlNodeList nodes = xmlDoc.SelectNodes(elementsTitlePath);

            foreach (XmlNode node in nodes)
            {
                XmlAttribute att = node.Attributes[catIdPath];

                string StandardId = att.InnerText;
                XmlNodeList virtualObservationNodes = LoadClassText(xmlDoc, StandardId);

                foreach (XmlNode item in virtualObservationNodes)
                {
                    //XmlAttribute att = node.Attributes[epIdPath];

                    string docText = node.InnerText;
                    TrainingDocument classDoc = new TrainingDocument(StandardId, docText, this.Tokenizer);
                        TrainingSet.Add(classDoc);

                }
            }

        }

        public void trainFromXML(string[] classFiles)
        {

            foreach (var observationFileName in classFiles)
            {
                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.Load(observationFileName);

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

                        TrainingDocument classDoc = new TrainingDocument(docClass, docText, this.Tokenizer);
                        saveToXML(classDoc);
                        TrainingSet.Add(classDoc);

                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classFiles"></param>
        public void TrainFromXmlSerialized(string[] classFiles)
        {

            foreach (var trainingDocumentFileName in classFiles)
            {
                TrainingDocument td = readFromXML(trainingDocumentFileName);
                //td.init();
                TrainingSet.Add(td);

                //TrainingDocument classDoc = new TrainingDocument(td.Class, td.Text, this.Tokenizer);
                //TrainingSet.Add(classDoc);
            }
        }


        private void saveToXML(TrainingDocument classDoc)
        {
            var path = @"C:\Atum Technology Group\AQS\NLP\TrainingSets\EC\XML\Data\fileName.xml".Replace("fileName", classDoc.Class);
            using (StreamWriter sw = new StreamWriter(path))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(TrainingDocument));

                xSer.Serialize(sw, classDoc);
            }
        }

        private TrainingDocument readFromXML(string path)
        {
            TrainingDocument retVal = null;

            using (StreamReader sr = new StreamReader(path)) 
            {
                XmlSerializer xSer = new XmlSerializer(typeof(TrainingDocument));

                retVal = (TrainingDocument)xSer.Deserialize(sr);
            }

            return retVal;
            
        }

        internal void trainFromChapterx(string chapterFileName)
        {
            //<title epid='EC.01.01.01'>The organization plans activities that minimize risks in the environment of care. Note: One or more persons can be assigned to manage risks associated with the management plans described in this standard.</title>
            //<element epid='EC.01.01.01' id='1'>1.	Leaders identify an individual(s) to manage risk, coordinate risk reduction activities in the environment of care, collect information on deficiencies, and disseminate summaries of actions and results.</element>
            //<note epid='EC.01.01.01' itemid='2'>Note 1: This information is disseminated to individuals with responsibility for the issues being addressed.</note>

            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(chapterFileName);
            //chapter><titles>
            string titlesMatch = "chapter/titles[title]/*";
            string elementsMatch = "chapter/elements[element]/*";
            string notessMatch = "chapter/notes[note]/*";


            loadMatcheDocs(titlesMatch,xmlDoc);
            loadMatcheDocs(elementsMatch, xmlDoc);
            loadMatcheDocs(notessMatch, xmlDoc);

        }

        private void loadMatcheDocs(string titlesMatch, System.Xml.XmlDocument xmlDoc)
        {
            XmlNodeList docNodes = xmlDoc.SelectNodes(titlesMatch);
            foreach (XmlNode item in docNodes)
            {
                string docClass = item.SelectSingleNode("@epid").InnerText;
                string docText = item.InnerText;

                TrainingDocument classDoc = new TrainingDocument(docClass, docText, this.Tokenizer);
                TrainingSet.Add(classDoc);

            }
        }


        private static bool isExcluded(string word)
        {
            List<string> wordsToExclude = getExcludedWords();
            bool retVal = getExcludedWords().Contains(word);
            WordPOS.POS wPOS = WordPOS.WordPos(word);

            switch (wPOS)
            {
                case WordPOS.POS.Adjective:
                    break;
                case WordPOS.POS.Noun:
                    break;
                case WordPOS.POS.Adverb:
                    break;
                case WordPOS.POS.Verb:
                    break;
                case WordPOS.POS.Pronoun:
                    retVal |= true;
                    break;
                case WordPOS.POS.Conjunction:
                    retVal |= true;
                    break;
                case WordPOS.POS.Preposition:
                    retVal |= true;
                    break;
                //a word added to a sentence to convey an emotion or a sentiment 
                case WordPOS.POS.Interjection:
                    retVal |= true;
                    break;
                case WordPOS.POS.Idiom:
                    break;
                case WordPOS.POS.Other:
                    retVal |= true;
                    break;
                default:
                    break;
            }
            return retVal;
        }

        private static string getEP(string line)
        {
            var EP = Regex.Match(line, MATCH_FINDING_EP).Value.Remove(0, 4);

            return EP;
        }

        private static bool matchFound(string line, string matchExpression)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(matchExpression);
            return regex.IsMatch(line);
        }

        public TrainingSet TrainingSet { get; set; }


        public string Classify(string observation)
        {
            Dictionary<string, ClassificationProbalities> logProbsByClass = new Dictionary<string, ClassificationProbalities>();
            ClassificationProbalities maxClassProb = null;
            ClassificationProbalities curClassProb = null;
            ClassDocument observationDoc = new ClassDocument(observation, this.Tokenizer);

                foreach (var classId in TrainingSet.TSetClasses)
                {
                    foreach (string word in observationDoc.Words)
                    {
                        decimal condProb = TrainingSet.CalculateConditionalProbability(word, classId);
                        double logProb = Math.Log((double)condProb);
                        double prior = this.Priors()[classId];

                        if (logProbsByClass.ContainsKey(classId))
                        {
                            logProbsByClass[classId].AddLogProb(logProb);
                            curClassProb = logProbsByClass[classId];
                        }
                        else
                        {
                            ClassificationProbalities cprob = new ClassificationProbalities(classId, prior);
                            cprob.AddLogProb(logProb);
                            logProbsByClass.Add(classId, cprob);
                            curClassProb = cprob;

                            if (maxClassProb == null)
                            {
                                maxClassProb = cprob;
                            }
                        }

                        maxClassProb = (curClassProb.Probability > maxClassProb.Probability) ? curClassProb : maxClassProb;
                    }
                }

            return getMaxtClass(logProbsByClass);
        }

        private string getMaxtClass(Dictionary<string, ClassificationProbalities> logProbsByClass)
        {
            string retVal = "";
            double maxProb = 0;
            foreach (var item in logProbsByClass.Values)
            {
                if (item.Probability > maxProb)
                {
                    maxProb = item.Probability;
                    retVal = item.Class;
                }
            }

            return retVal;
        }

        private Dictionary<string, double> Priors()
        {

            Dictionary<string, double> retVal = new Dictionary<string, double>();

            int documentCount = this.TrainingSet.TrainingDocuments.Count;
            foreach (var item in this.TrainingSet.TSetClasses)
            {
                int documentsInClass = this.TrainingSet.ClassDocuments[item];
                double prior = (double)documentsInClass / (documentCount);
                retVal.Add(item, prior);

            }

            return retVal;
        }

        public Tokenizer Tokenizer { get; set; }

    }
}
