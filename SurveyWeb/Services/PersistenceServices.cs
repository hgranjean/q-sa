using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using Atum.Domain.Basis;
using Atum.Utility.XML;
using SurveyWeb.Models;
using Atum.Domain.QualityManagement;
using Atum.Domain.SurveyManagement;

namespace SurveyWeb.Services
{
    internal class PersistenceServices
    {
        private static List<Survey> surveys; 
        private static SurveyManager _surverManager;
        
        public SurveyManager GetSurveyManager(Survey survey)
        {
            _surverManager = new SurveyManager(survey);
            return _surverManager;
        }

        public IEnumerable<Survey> GetSurveys()
        {
            if (surveys == null)
            {
                surveys = new List<Survey>();
                foreach (var surveyFileName in EnumerateSurveys())
                {
                    surveys.Add(LoadSurvey(surveyFileName));
                }
            }

            return surveys;
        }

        public IEnumerable<string> GetResponses()
        {
            var responses = EnumerateResponses();

            return responses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Surveys GetSurveys(string criteria)
        {
            return new Surveys();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Survey GetSurvey(int id)
        {
            return GetSurveys().FirstOrDefault(survey => survey.ID == id);
        }


        public Survey LoadSurvey(string name)
        {
            return (Survey)XmlSerializationUtility.GetObjectFromFile(name, typeof(Survey));
        }

        public void SaveSurvey(Survey survey)
        {
            survey.RenumberQuestions();

            var appPath = GetAppPath();

            var fileName = String.Concat("survey", survey.ID, ".xml");

            var settings = new XmlWriterSettings {Indent = true};
            
            using (var writer = XmlWriter.Create(Path.Combine(appPath, fileName), settings))
            {
                XmlSerializationUtility.ObjectToXmlWriter(writer, survey);
            }

            EnsureSurveys();

            AddOrUpdateSurvey(survey);
        }

        public void SaveTracer(TracerViewModel survey)
        {
            survey.UpdatedDate = DateTime.Now;

            var appPath = GetAppPath();

            var fileName = String.Concat("response", survey.ResponseId, ".xml");

            var settings = new XmlWriterSettings { Indent = true };
            
            using (var writer = XmlWriter.Create(Path.Combine(appPath, fileName), settings))
            {
                XmlSerializationUtility.ObjectToXmlWriter(writer, survey);
            }
        }

        public TracerViewModel LoadTracer(string responseId)
        {
            var appPath = GetAppPath();

            var fullPath = Path.Combine(appPath, "response" + responseId + ".xml");

            return (TracerViewModel)XmlSerializationUtility.GetObjectFromFile(fullPath, typeof(TracerViewModel));
        }

        private void SetSurveyId(Survey survey)
        {
            if (survey.ID == DomainObject.DefaultIdentifier)
            {
                survey.AssignNextId(EnumerateSurveys().Count());
            }
        }

        private void EnsureSurveys()
        {
            if (surveys == null)
            {
                surveys = new List<Survey>();
            }
        }

        private void AddOrUpdateSurvey(Survey survey)
        {
            if (!surveys.Contains(survey))
            {
                surveys.Add(survey);
            }
            else
            {
                var index = surveys.IndexOf(survey);

                surveys[index] = survey;
            }

            SetSurveyId(survey);
        }

        private string[] EnumerateSurveys()
        {
            var appPath = GetAppPath();
            
            return Directory.GetFiles(appPath, "survey*.xml");
        }
        
        private string[] EnumerateResponses()
        {
            var appPath = GetAppPath();
            
            return Directory.GetFiles(appPath, "response*.xml");
        }

        private string GetAppPath()
        {
            string appPath = HttpContext.Current.Server.MapPath("~/bin");
            
            int binPos = appPath.LastIndexOf(@"\bin", StringComparison.CurrentCultureIgnoreCase);

            appPath = appPath.Substring(0, binPos) + @"\RuleApp\";
            
            return appPath;
        }

        internal void DeleteSurvey(string id)
        {   
            var toDelete = surveys.FirstOrDefault(survey => survey.ID == Int32.Parse(id));

            if (surveys.Contains(toDelete))
            {
                surveys.Remove(toDelete);
            }

            var appPath = GetAppPath();

            var fullPath = Path.Combine(appPath, "survey" + toDelete.ID + ".xml");
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);    
            }
        }
    }
}