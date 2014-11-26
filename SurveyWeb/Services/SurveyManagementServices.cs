using Atum.Domain.SurveyManagement;
using SurveyWeb.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Services
{
    public class SurveyManagementServices
    {
        private readonly ISurveyRepository _repository;        
        public SurveyManagementServices(ISurveyRepository repository)
        {
            _repository = repository;
        }

        //Should Use Reposistory Capabilty of getting Surveys by Criteria
        internal IEnumerable<Survey> GetManagerSurveys(int ManagerId)
        {
            var query = "";
            //Construct Criteria and pass to repository
            return _repository.GetSurveys(query);
        }

        internal Survey SaveSurvey(Survey survey)
        {
            return _repository.SaveSurvey(survey);
        }

        internal Survey GetSurvey(int surveyId)
        {
            return _repository.GetSurvey(surveyId);
        }



        internal SurveyEntry GetSurveyEntry(string p)
        {
            throw new NotImplementedException();
        }
    }
}