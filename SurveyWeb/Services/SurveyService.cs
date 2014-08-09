using Atum.Domain.SurveyManagement;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class SurveyService
    {
        private readonly ISurveyRepository _repository;        
        public SurveyService(ISurveyRepository repository)
        {
            _repository = repository;
        }
        internal SurveyEntry GetSurvey(string surveyId)
        {
            return _repository.GetSurvey(surveyId);
        }

        internal SurveyEntry AddSurvey(SurveyEntry entry)
        {
            return _repository.AddSurvey(entry);
        }
    }
}