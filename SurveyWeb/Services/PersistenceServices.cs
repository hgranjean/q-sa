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
        private static List<Survey> _surveyList;
        private static SurveyManager _surverManager;
        private readonly ISurveyStore _store;

        public PersistenceServices(ISurveyStore store)
        {
            _store = store;
        }
        
        public SurveyManager GetSurveyManager(ManagedSurvey survey)
        {
            _surverManager = new SurveyManager(survey);
            return _surverManager;
        }

        public IEnumerable<Survey> GetSurveys()
        {   
            if (_surveyList == null)
            {
                EnsureSurveys();

                foreach (var surveyFileName in EnumerateSurveys())
                {
                    _surveyList.Add(LoadSurveyByName(surveyFileName));
                }
            }

            return _surveyList;
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


        private Survey LoadSurveyByName(string name)
        {
            //Get From DB
            
            return (Survey)XmlSerializationUtility.GetObjectFromFile(name, typeof(Survey));
        }

        public void SaveSurvey(Survey survey)
        {
            survey.RenumberQuestions();

            var fullPath = Path.Combine(_store.GetPath(StoreType.Surveys), "survey" + survey.Id + ".xml");

            var settings = new XmlWriterSettings {Indent = true};
            
            using (var writer = XmlWriter.Create(fullPath, settings))
            {
                XmlSerializationUtility.ObjectToXmlWriter(writer, survey);
            }

            AddOrUpdateSurvey(survey);
        }

        public void SaveTracer(TracerViewModel survey)
        {
            survey.UpdatedDate = DateTime.Now;

            var fullPath = Path.Combine(_store.GetPath(StoreType.Responses), "response" + survey.ResponseId + ".xml");

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
            if (_surveyList == null)
            {
                _surveyList = new List<Survey>();
            }
        }

        private void AddOrUpdateSurvey(Survey survey)
        {
            EnsureSurveys();
            if (!_surveyList.Contains(survey))
            {
                _surveyList.Add(survey);
            }
            else
            {
                var index = _surveyList.IndexOf(survey);

                _surveyList[index] = survey;
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

        public void DeleteSurvey(long id)
        {   
            var toDelete = _surveyList.FirstOrDefault(survey => survey.Id == id);
            
            if (toDelete != default(Survey))
            {
                _surveyList.Remove(toDelete);
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

        public void SaveNote(EditNoteViewModel viewModel)
        {
            var fullPath = Path.Combine(_store.GetPath(StoreType.Notes), "note" + viewModel.NoteId + ".xml");

            var settings = new XmlWriterSettings { Indent = true };

            using (var writer = XmlWriter.Create(fullPath, settings))
            {
                XmlSerializationUtility.ObjectToXmlWriter(writer, viewModel);
            }
        }


        public SurveyManager GetSurveyManager(Survey survey)
        {
            throw new NotImplementedException();
        }
        
        public EditNoteViewModel LoadNote(int surveyId, int questionGroupId, int questionId, Guid responseId)
        {
            foreach (var fileName in Directory.GetFiles(_store.GetPath(StoreType.Notes), "note*.xml"))
            {
                var viewModel =
                    (EditNoteViewModel) XmlSerializationUtility.GetObjectFromFile(fileName, typeof (EditNoteViewModel));

                if (viewModel.SurveyId == surveyId &&
                    viewModel.QuestionGroupNumber == questionGroupId &&
                    viewModel.QuestionId == questionId &&
                    viewModel.ResponseId  == responseId.ToString())
                {
                    return viewModel;
                }
            }

            return null;
        }
    }
}