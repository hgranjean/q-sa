using Atum.Domain.NLP;
using Atum.Domain.NLP.NaiveBayes;
using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using SurveyWeb.Mappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SurveyWeb.Services
{
    internal class LearningServices
    {

        EPClassifier GetClassifier(string classifierId)
        {
            EPClassifier retVal = null;
            Dictionary<string, EPClassifier> classifierMap = this.ClassifierMap;

            if (!classifierMap.ContainsKey(classifierId))
            {
                
            }

            return retVal;  
        }

        private Dictionary<string, EPClassifier> ClassifierMap { get; set; }

        internal Models.StandardElementViewModel Classify(string observation)
        {
            //TODO: Service should return model, not viewModel!

            string observationClass = GetClassifier().Classify(observation);

            Models.StandardElementViewModel retVal = new Models.StandardElementViewModel();

            Standard standard = GetStandardChapter().GetPerformanceCategory(observationClass);
            
            retVal.StandardId = observationClass;
            retVal.Content = standard.Title ;
            retVal.EPIds = LoadEPIds(standard.Items);

            return retVal;
        }

        private string GetStandardChapter(string observationClass)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<string> LoadEPIds(List<ElementOfPerformance> epIds)
        {
            List<string> eps = new List<string>();
            IEnumerable<string> retVal = eps;

            //var epIds = GetStandardChapter().GetPerformanceCategory(observationClass).Items;

            foreach (var epId in epIds)
            {
                string strToAdd = string.Format("EP {0}", epId.EPId);
                eps.Add(strToAdd);
            }

            return retVal;
        }


        static Chapter standardChapter;
        private static Chapter GetStandardChapter()
        {
            if (standardChapter==null)
            {
                standardChapter = StandardsManagementServices.loadChapter("EC");
            }

            return standardChapter;
        }

        static EPClassifier classifier;
        /// <summary>
        /// TODO: Add Option to 
        /// </summary>
        /// <returns></returns>
        private static EPClassifier GetClassifier()
        {
            if (classifier==null)
            {
             
                string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/");
                string modelPath = HttpContext.Current.Server.MapPath("~/OpenNLP/Models/");
                classifier = new EPClassifier(modelPath);

                string[] xmlClassFiles = System.IO.Directory.GetFiles(appPath);
                //classifier.trainFromXML(xmlClassFiles);

                //string chapterFileName = @"C:\Atum Technology Group\Rules Venture\Reference Docs\Joint Commision Standards\EC_out.xml";

                //classifier.trainFromChapter(chapterFileName);

                appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/Serialized/");
                xmlClassFiles = System.IO.Directory.GetFiles(appPath);
                
                classifier.trainFromXMLSerialized(xmlClassFiles);

            }

            return classifier;
        }


        internal Models.TrainingDocumentViewModel GetTrainingDocument(string trainingDocumentId)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/Serialized/");
            string filePath = appPath + trainingDocumentId + ".xml";

            TrainingDocument trainingDoc = readFromXML(filePath);
            Models.TrainingDocumentViewModel retVal = Learning.MapToViewModel(trainingDoc);

            retVal.ClassList = loadClassList(StandardsManagementServices.GetChapter("EC"));

            return retVal;

        }

        private IEnumerable<string> loadClassList(Models.StandardDocumentViewModel standardDocumentViewModel)
        {
            List<string> retVal = new List<string>();

            foreach (var item in standardDocumentViewModel.TableOfContents)
            {
                string classKey = item.Key;
                retVal.Add(classKey);
            }

            return retVal;
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


        internal Models.TrainingDocumentViewModel SaveTrainingDocument(Models.TrainingDocumentViewModel model)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/");
            string modelPath = HttpContext.Current.Server.MapPath("~/OpenNLP/Models/");
            Atum.Domain.NLP.Tokenizer Tokenizer = new Tokenizer(EPClassifier.getExcludedWords(), modelPath);

            TrainingDocument classDoc = Learning.MapToDomainModel(model, Tokenizer);
            saveToXML(classDoc);
            Models.TrainingDocumentViewModel retVal = Learning.MapToViewModel(classDoc);
            retVal.ClassList = loadClassList(StandardsManagementServices.GetChapter("EC"));

            return retVal;
        }

        private void saveToXML(TrainingDocument classDoc)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/Serialized/");
            string filePath = appPath + classDoc.Class + ".xml";

            using (StreamWriter sw = new StreamWriter(filePath))
            {
                XmlSerializer xSer = new XmlSerializer(typeof(TrainingDocument));

                xSer.Serialize(sw, classDoc);
            }
        }


    }
}