using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Atum.Domain.Surveillance;
using Atum.Utility.XML;

namespace SurveyWeb
{
    internal class SurveillanceServices
    {
        private static List<Survey> surveys; 
        private static SurveyManager _surverManager;
        
        static SurveillanceServices()
        {
            
        }
        
        public static SurveyManager GetSurveyManager(Survey survey)
        {
            _surverManager = new SurveyManager(survey);
            return _surverManager;
        }

        public static IEnumerable<Survey> GetSurveys()
        {
            if (surveys == null)
            {
                surveys = new List<Survey>();
                foreach (var surveyFileName in EnumerateSurveys())
                {
                    surveys.Add(LoadSurvey(surveyFileName));
                }
                // surveys.AddRange(new [] { new Survey(), new Survey(), new Survey() });
            }

            return surveys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Surveys GetSurveys(string criteria)
        {
            return new Surveys();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Survey GetSurvey(int id)
        {
            return new Survey();
        }


        public static Survey LoadSurvey(string name)
        {
            return (Survey)XmlSerializationUtility.GetObjectFromFile(name, typeof(Survey));
        }

        public static void Save(Survey survey)
        {
            if (surveys == null)
            {
                surveys = new List<Survey>();
            }
            if (!surveys.Contains(survey))
            {
                surveys.Add(survey);
            }

            var appPath = GetAppPath();

            //if (survey.ID == -1)
            //{
            //    survey.AssignNextId(EnumerateSurveys().Count());
            //}

            //using (var writer = XmlWriter.Create(Path.Combine(appPath, "survey" + survey.ID)))
            //{
            //    XmlSerializationUtility.ObjectToXmlWriter(writer, survey);
            //}
        }

        private static string[] EnumerateSurveys()
        {
            var appPath = GetAppPath();

            // return appPath + ConfigurationSettings.AppSettings.Get("RuleApp");

            return Directory.GetFiles(appPath, "*.xml");
        }

        private static string GetAppPath()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");
            int binPos = appPath.LastIndexOf(@"\bin");

            appPath = appPath.Substring(0, binPos) + @"\RuleApp\";
            return appPath;
        }
    }
}