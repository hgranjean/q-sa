using System.Collections.Generic;
using Atum.Domain.Surveillance;

namespace SurveyWeb.Models
{
    public class SurveyViewModel
    {
        public Survey Survey { get; private set; }

        public SurveyViewModel(Survey survey)
        {
            this.Survey = survey;
        }

        public static IEnumerable<Survey> GetSurveys()
        {
            return SurveyServices.GetSurveys();
        }

        public void Save()
        {
            SurveyServices.Save(this.Survey);
        }
    }
}