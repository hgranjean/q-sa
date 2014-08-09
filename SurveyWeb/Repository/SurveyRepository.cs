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
        SurveyEntry GetSurvey(string surveyId);

        SurveyEntry AddSurvey(SurveyEntry entry);
    }

    public class SurveyRepository : ISurveyRepository
    {
        private readonly AtumSurveillanceContext _context;
        
        public SurveyRepository(string connectionString)
        {
            _context = new AtumSurveillanceContext();
        }

        public SurveyEntry GetSurvey(string surveyId)
        {
            return _context.Surveys.FirstOrDefault(m => m.Id == surveyId);
        }


        public SurveyEntry AddSurvey(SurveyEntry entry)
        {
            _context.Surveys.Add(entry);

            _context.SaveChanges();

            return entry;
        }
    }
}