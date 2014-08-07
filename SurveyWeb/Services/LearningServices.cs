using Atum.Domain.NLP;
using Atum.Domain.NLP.NaiveBayes;
using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using Common.Utilities;
using SurveyWeb.Mappers;
using SurveyWeb.Models;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SurveyWeb.Services
{
    public class LearningServices
    {
        private readonly ISurveyStore _store;
        private readonly StandardsManagementServices _standardManagementService;
        private Dictionary<string, EPClassifier> ClassifierMap { get; set; }

        public LearningServices(ISurveyStore store, StandardsManagementServices standardManagementService)
        {
            _store = store;
            _standardManagementService = standardManagementService;
        }

        private EPClassifier GetClassifier(string classifierId)
        {
            EPClassifier retVal = null;
            Dictionary<string, EPClassifier> classifierMap = this.ClassifierMap;

            if (!classifierMap.ContainsKey(classifierId))
            {
                // [aschmidt] What's this?
            }

            return retVal;  
        }        

        internal StandardElementViewModel Classify(string observation)
        {
            //TODO: Service should return model, not viewModel!
            string observationClass = GetClassifier().Classify(observation);

            Standard standard = GetStandardChapter(observationClass).GetPerformanceCategory(observationClass);

            var retVal = new StandardElementViewModel();
            retVal.StandardId = observationClass;
            retVal.Content = standard.Title ;
            retVal.EPIds = LoadEPIds(standard.Items);

            return retVal;
        }
            
        private IEnumerable<string> LoadEPIds(string observationClass)
        {
            throw new NotImplementedException();
        }
    
        private static IEnumerable<string> LoadEPIds(List<ElementOfPerformance> epIds)
        {
            List<string> eps = new List<string>();
            IEnumerable<string> retVal = eps;
            
            // var standard = GetStandardChapter(observationClass).GetPerformanceCategory(observationClass);

            // var epIds = standard.Items;

            return epIds.ConvertAll(m => string.Format("EP {0}", m.EPId));
        }


        private static Chapter s_standardChapter;
        private Chapter GetStandardChapter(string observationClass)
        {
            if (s_standardChapter == null)
            {
                s_standardChapter = _standardManagementService.LoadChapter(observationClass);
            }

            return s_standardChapter;
        }

        private static EPClassifier s_classifier;
        /// <summary>
        /// TODO: Add Option to 
        /// </summary>
        /// <returns></returns>
        private EPClassifier GetClassifier()
        {
            if (s_classifier == null)
            {
             
                var standardsPath = Path.Combine(_store.GetPath(), "JointCommissionStandards/Training/EC");
                var modelPath = Path.Combine(_store.GetPath(), "OpenNLP/Models");
                
                s_classifier = new EPClassifier(modelPath);

                var xmlClassFiles = Directory.GetFiles(standardsPath);
                
                standardsPath = Path.Combine(_store.GetPath(), "JointCommissionStandards\\Training\\EC\\Serialized");
                
                xmlClassFiles = Directory.GetFiles(standardsPath);
                
                s_classifier.TrainFromXmlSerialized(xmlClassFiles);
            }

            return s_classifier;
        }


        internal Models.TrainingDocumentViewModel GetTrainingDocument(string trainingDocumentId)
        {
            string appPath = HttpContext.Current.Server.MapPath("~/Content/JointCommissionStandards/Training/EC/Serialized/");
            string filePath = appPath + trainingDocumentId + ".xml";

            TrainingDocument trainingDoc = ReadFromXML(filePath);
            Models.TrainingDocumentViewModel retVal = Learning.MapToViewModel(trainingDoc);

            retVal.ClassList = LoadClassList(_standardManagementService.GetChapter("EC"));

            return retVal;

        }

        private IEnumerable<string> LoadClassList(Models.StandardDocumentViewModel standardDocumentViewModel)
        {
            List<string> retVal = new List<string>();

            foreach (var item in standardDocumentViewModel.TableOfContents)
            {
                string classKey = item.Key;
                retVal.Add(classKey);
            }

            return retVal;
        }

        private TrainingDocument ReadFromXML(string path)
        {
            return (TrainingDocument)XmlSerializationUtility.GetObjectFromFile(path, typeof(TrainingDocument));
        }

        internal TrainingDocumentViewModel SaveTrainingDocument(TrainingDocumentViewModel model)
        {
            var appPath = Path.Combine(_store.GetPath(), "JointCommissionStandards\\Training\\EC");
            var modelPath = Path.Combine(_store.GetPath(), "OpenNLP\\Models");
            Tokenizer Tokenizer = new Tokenizer(EPClassifier.getExcludedWords(), modelPath);

            var classDoc = Learning.MapToDomainModel(model, Tokenizer);
            SaveToXML(classDoc);
            
            var retVal = Learning.MapToViewModel(classDoc);
            retVal.ClassList = LoadClassList(_standardManagementService.GetChapter("EC"));
            
            return retVal;
        }

        private void SaveToXML(TrainingDocument classDoc)
        {
            string appPath = Path.Combine(_store.GetPath(), "JointCommissionStandards\\Training\\EC\\Serialized");
            string filePath = Path.Combine(appPath, classDoc.Class + ".xml");

            XmlSerializationUtility.SaveObjectToFile(filePath, classDoc);            
        }
    }
}