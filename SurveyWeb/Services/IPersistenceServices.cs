using System;
using System.Collections.Generic;
using Atum.Domain.SurveyManagement;
using SurveyWeb.Models;

namespace SurveyWeb.Services
{
    public interface IPersistenceServices
    {
        IEnumerable<string> GetResponses();
        Survey GetSurvey(int id);
        SurveyManager GetSurveyManager(Survey survey);
        IEnumerable<Survey> GetSurveys();
        Surveys GetSurveys(string criteria);
        Survey LoadSurvey(string name);
        TracerViewModel LoadTracer(string responseId);
        void SaveSurvey(Survey survey);
        void SaveTracer(TracerViewModel survey);
        void DeleteSurvey(string surveyId);
        void DeleteResponse(string responseId);
    }
}
