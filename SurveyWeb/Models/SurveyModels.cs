using System.Collections.Generic;
using Atum.Domain.Surveillance;

namespace SurveyWeb.Models
{
    public class SurveyViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public Survey Survey { get; private set; }

        public SurveyViewModel()
        {
            this.Survey = new Survey();
        }

        public SurveyViewModel(Survey survey)
        {
            this.Name = this.Name ?? "Survey" + survey.ID;
            this.Survey = survey;
            

            if (this.Survey.QuestionGroups == null)
            {
                this.Survey.AddQuestionGroup("One");
            }
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