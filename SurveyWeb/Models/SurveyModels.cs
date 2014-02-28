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
            this.Name = this.Name ;//?? "Survey" + survey.ID;
            this.Survey = survey;
            

            if (this.Survey.QuestionGroups == null)
            {
                this.Survey.AddQuestionGroup("One");
            }
        }


        
        public static IEnumerable<Survey> GetSurveys()
        {
            return SurveillanceServices.GetSurveys();
        }

        public void Save()
        {
            SurveillanceServices.Save(this.Survey);
        }

        public void AddQuestionGroup()
        {
            if (Survey.QuestionGroups == null)
            {
                Survey.QuestionGroups = new QuestionGroups();
            }
            
            int newGroupIndex = Survey.QuestionGroups.Count + 1;

            Survey.AddQuestionGroup("New Group " + newGroupIndex);
        }
    }

    public class SurveyViewModels : List<Survey>
    {

    }

    public class QuestionGroupViewModel : ViewModelBase
    {
        public string SurveyId { get; set; }
        public int Number { get; set; }
        public QuestionGroup QuestionGroup { get; set; }
        public Questions Questions { get; set; }

        public QuestionGroupViewModel()
        {
            
        }

        public QuestionGroupViewModel(QuestionGroup questionGroup)
        {
            this.QuestionGroup = questionGroup;
            this.Questions = questionGroup.Questions;
            this.Number = questionGroup.Number;
        }
    }
}