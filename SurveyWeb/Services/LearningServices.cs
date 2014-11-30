using Atum.Domain.NLP;
using Atum.Domain.NLP.NaiveBayes;
using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using Atum.Utility.XML;
using SurveyWeb.Mappers;
using SurveyWeb.Models;
using SurveyWeb.Models.StandardMaintenance;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SurveyWeb.Services
{
    public class LearningServices
    {
        private readonly ISurveyStore _store;
        private readonly StandardsManagementServices _standardManagementService;
                
        private Dictionary<string, EPClassifier> ClassifierMap { get; set; }

        // TODO: Refactor to Cache
        private static Dictionary<string, string> _standardDefaultClassLookUp;

        public LearningServices(ISurveyStore store, StandardsManagementServices standardManagementService)
        {
            _store = store;
            _standardManagementService = standardManagementService;
        }

        private EPClassifier GetClassifier(string classifierId)
        {
            EPClassifier retVal = null;            

            if (!this.ClassifierMap.ContainsKey(classifierId))
            {
                // [aschmidt] What's this?
            }

            return retVal;  
        }        

        internal StandardElement Classify(string observation)
        {        
            string observationClass = GetClassifier().Classify(observation);

            var standard = GetStandardChapter(observationClass)
                .GetPerformanceCategory(observationClass);

            var model = new StandardElement();
            model.StandardId = observationClass;
            model.Content = standard.Title;
            model.EPIds = LoadEPIds(standard.Items);
            
            return model;
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

        //TODO Load multiple chapters
        //private static Chapter s_standardChapter;
        private Chapter GetStandardChapter(string chapterId)
        {
            //if (s_standardChapter == null)
            //{
            //    s_standardChapter = _standardManagementService.LoadChapter(observationClass);
            //}

            //return s_standardChapter;


            return _standardManagementService.LoadChapter(chapterId);
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
             
                var standardsPath = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "Training/EC");
                var modelPath = Path.Combine(_store.GetPath(StoreType.OpenNLP), "Models");
                
                s_classifier = new EPClassifier(modelPath);

                var xmlClassFiles = Directory.GetFiles(standardsPath);
                
                standardsPath = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "Training\\EC\\Serialized");
                
                xmlClassFiles = Directory.GetFiles(standardsPath);
                
                s_classifier.TrainFromXmlSerialized(xmlClassFiles);
            }

            return s_classifier;
        }


        internal Models.TrainingDocumentViewModel GetTrainingDocument(string chapterId, string trainingDocumentId)
        {   
            string appPath = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "Training/EC/Serialized");
            string filePath = Path.Combine(appPath, trainingDocumentId + ".xml");


            TrainingDocument trainingDoc = ReadFromXML(filePath);
            Models.TrainingDocumentViewModel retVal = Learning.MapToViewModel(trainingDoc);

            StandardDocumentViewModel stdViewModel = _standardManagementService.GetChapter(chapterId);
            Standard standard = GetStandardChapter(chapterId).GetPerformanceCategory(trainingDocumentId);
            
            retVal.ClassList = LoadClassList(stdViewModel);
            retVal.StandardText = LoadStandardText(standard);
            retVal.StandardTitle = standard.Title;

            return retVal;

        }

        private string LoadStandardText(Standard standard)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(standard.Title);

            foreach (var item in standard.Items)
            {
                sb.AppendLine(item.Text);
                foreach (var noteItem in item.Notes)
                {
                    sb.AppendLine("\t"+ noteItem);
                }
            }
            string retVal = sb.ToString();

            return retVal;

        }

        private IEnumerable<string> LoadClassList(StandardDocumentViewModel standardDocumentViewModel)
        {
            foreach (var item in standardDocumentViewModel.TableOfContents)
            {
                yield return item.Key;                
            }
        }

        private TrainingDocument ReadFromXML(string path)
        {
            return (TrainingDocument)XmlSerializationUtility.GetObjectFromFile(path, typeof(TrainingDocument));
        }

        const string standardBody = "JointCommissionStandards";
        const string standardChapter = "EC";

        //TODO: Fix hardcoded strings
        internal TrainingDocumentViewModel SaveTrainingDocument(TrainingDocumentViewModel model)
        {   
            var appPath = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "\\Training\\EC");
            var modelPath = Path.Combine(_store.GetPath(StoreType.OpenNLP), "Models");
            
            var tokenizer = new Tokenizer(EPClassifier.getExcludedWords(), modelPath);

            var classDoc = Learning.MapToDomainModel(model, tokenizer);
            SaveToXML(classDoc);
            
            var retVal = Learning.MapToViewModel(classDoc);
            retVal.ClassList = LoadClassList(_standardManagementService.GetChapter("EC"));
            
            return retVal;
        }

        //TODO Extend to other Chapters
        private void SaveToXML(TrainingDocument classDoc)
        {
            string appPath = Path.Combine(_store.GetPath(StoreType.JointCommissionStandards), "Training\\EC\\Serialized");
            string filePath = Path.Combine(appPath, classDoc.Class + ".xml");

            XmlSerializationUtility.SaveObjectToFile(filePath, classDoc);            
        }
                
        internal string GetDefaultTrainingDocumentId(string standardId)
        
        {
            if (_standardDefaultClassLookUp == null)
            {
                _standardDefaultClassLookUp = new Dictionary<string, string>();
                _standardDefaultClassLookUp.Add(standardChapter, standardChapter + Type.Delimiter);                
            }

            return _standardDefaultClassLookUp[standardId];
        }
    }
}