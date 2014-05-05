using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SurveyWeb.Controllers
{
    public class CompletedSurveyViewModel
    {
        public IEnumerable<string> CompletedSurveys { get; set; }
        public IEnumerable<string> CompletedSurveysShortNames { get; set; } 

        public CompletedSurveyViewModel()
        {}

        public CompletedSurveyViewModel(IEnumerable<string> completedSurveys)
        {
            this.CompletedSurveys = completedSurveys;

            var completedSurveysShortNames = new List<string>();

            foreach (var surveyName in completedSurveys)
            {
                completedSurveysShortNames.Add(System.IO.Path.GetFileName(surveyName));
            }

            this.CompletedSurveysShortNames = completedSurveysShortNames;
        }
    }
}
