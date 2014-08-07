using Atum.Domain.NLP.NaiveBayes;
using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using SurveyWeb.Models;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{   public class LearningServices
    {
        private readonly ISurveyStore _store;
        private readonly StandardsManagementServices _standardManagementService;

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
                
            }

            return retVal;  
        }

        private Dictionary<string, EPClassifier> ClassifierMap { get; set; }

        internal StandardElementViewModel Classify(string observation)
        {
            //TODO: Service should return model, not viewModel!

            string observationClass = GetClassifier().Classify(observation);

            StandardElementViewModel retVal = new StandardElementViewModel();
            retVal.StandardId = observationClass;
            retVal.EPIds = LoadEPIds(observationClass);

            return retVal;
        }

        private IEnumerable<string> LoadEPIds(string observationClass)
        {
            var standard = GetStandardChapter().GetPerformanceCategory(observationClass);

            var epIds = standard.Items;

            return epIds.ConvertAll(m => string.Format("EP {0}", m.EPId));
        }


        private static Chapter s_standardChapter;
        private Chapter GetStandardChapter()
        {
            if (s_standardChapter == null)
            {
                s_standardChapter = _standardManagementService.LoadChapter("EC");
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
                
                //classifier.trainFromXML(xmlClassFiles);

                //string chapterFileName = @"C:\Atum Technology Group\Rules Venture\Reference Docs\Joint Commision Standards\EC_out.xml";

                //classifier.trainFromChapter(chapterFileName);

                standardsPath = Path.Combine(_store.GetPath(), "JointCommissionStandards\\Training\\EC\\Serialized");
                
                xmlClassFiles = Directory.GetFiles(standardsPath);
                
                s_classifier.TrainFromXmlSerialized(xmlClassFiles);

            }

            return s_classifier;
        }

    }
}