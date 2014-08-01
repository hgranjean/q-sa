using Atum.Domain.NLP.NaiveBayes;
using Atum.Domain.QualityManagement.Healthcare.JointCommission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            retVal.StandardId = observationClass;
            retVal.EPIds = LoadEPIds(observationClass);

            return retVal;
        }

        private static IEnumerable<string> LoadEPIds(string observationClass)
        {
            List<string> eps = new List<string>();
            IEnumerable<string> retVal = eps;

            var epIds = GetStandardChapter().GetPerformanceCategory(observationClass).Items;

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

    }
}