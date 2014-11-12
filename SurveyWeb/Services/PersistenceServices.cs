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
using SurveyWeb.Repository;

namespace SurveyWeb.Services
{
    internal class PersistenceServices : IPersistenceServices
    {
        private static List<Survey> surveys;
        private static SurveyManager _surverManager;
        private readonly ISurveyStore _store;

        public PersistenceServices(ISurveyStore store)
        {
            _store = store;
        }
        
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
            return GetSurveys().FirstOrDefault(survey => survey.Id == id);
        }


        public Survey LoadSurvey(string name)
        {
         
            //Get From DB
            
            
            return (Survey)XmlSerializationUtility.GetObjectFromFile(name, typeof(Survey));
        }

        public void SaveSurvey(Survey survey)
        {
            survey.RenumberQuestions();

            var appPath = _store.GetPath(StoreType.Surveys);

            var fileName = String.Concat("survey", survey.Id, ".xml");

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

            var appPath = _store.GetPath(StoreType.Responses);

            var fullPath = Path.Combine(appPath, "response" + survey.ResponseId + ".xml");

            var settings = new XmlWriterSettings { Indent = true };
            
            using (var writer = XmlWriter.Create(fullPath, settings))
            {
                XmlSerializationUtility.ObjectToXmlWriter(writer, survey);
            }
        }

        public TracerViewModel LoadTracer(string responseId)
        {
            var appPath = _store.GetPath(StoreType.Responses);

            var fullPath = Path.Combine(appPath, "response" + responseId + ".xml");

            return (TracerViewModel)XmlSerializationUtility.GetObjectFromFile(fullPath, typeof(TracerViewModel));
        }

        private void SetSurveyId(Survey survey)
        {
            if (survey.Id == DomainObject.DefaultIdentifier)
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
            return Directory.GetFiles(_store.GetPath(StoreType.Surveys), "survey*.xml");
        }
        
        private string[] EnumerateResponses()
        {             
            return Directory.GetFiles(_store.GetPath(StoreType.Responses), "response*.xml");
        }

        public void DeleteSurvey(string id)
        {   
            var toDelete = surveys.FirstOrDefault(survey => survey.Id == Int32.Parse(id));

            if (surveys.Contains(toDelete))
            {
                surveys.Remove(toDelete);
            }
            
            var fullPath = Path.Combine(_store.GetPath(StoreType.Surveys), "survey" + toDelete.Id + ".xml");
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);    
            }
        }

        public void DeleteResponse(string id)
        {            
            var fullPath = Path.Combine(_store.GetPath(StoreType.Responses), "response" + id + ".xml");

            // if (File.Exists(fullPath))
            //{
                File.Delete(fullPath);
            //}
        }
    }
}