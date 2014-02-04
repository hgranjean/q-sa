using System.Collections.Generic;
using Atum.Domain.Surveillance;

namespace SurveyWeb.Models
{
    public class SurveyViewModel
    {
        public Survey Survey { get; set; }

        public SurveyViewModel(Survey survey)
        {
            this.Survey = survey;
        }

        public static IEnumerable<Survey> GetSurveys()
        {
            yield return new Survey();
            yield return new Survey();
            yield return new Survey();
        }
    }
}