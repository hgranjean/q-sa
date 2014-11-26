using Atum.Database.Surveillance.Models;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Repository
{
    public interface ISurveyRepository
    {
        SurveyEntry GetSurveyEntry(string surveyId);

        SurveyEntry AddSurvey(SurveyEntry entry);

        Survey SaveSurvey(Survey survey);

        Survey GetSurvey(int surveyId);

        IEnumerable<Survey> GetSurveys(string query);
    }

    public class SurveyRepository : ISurveyRepository
    {
        private readonly AtumSurveillanceContext _context;
        
        public SurveyRepository(string connectionString)
        {
            _context = new AtumSurveillanceContext();
        }

        public SurveyEntry GetSurveyEntry(string surveyId)
        {
            return _context.Surveys.FirstOrDefault(m => m.Id == surveyId);
        }


        public SurveyEntry AddSurvey(SurveyEntry entry)
        {
            _context.Surveys.Add(entry);

            _context.SaveChanges();

            return entry;
        }


        public Survey SaveSurvey(Survey survey)
        {
            throw new NotImplementedException();
        }

        public Survey GetSurvey(int surveyId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Survey> GetSurveys(string query)
        {
            throw new NotImplementedException();
        }
    }
}