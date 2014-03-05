using System.Collections.Generic;
using Atum.Domain.Surveillance;

namespace SurveyWeb.Models
{
    public class SurveyViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public Survey Survey { get; set; }
        public QuestionGroupsViewModel QuestionGroupsViewModel { get; set; }

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

            this.QuestionGroupsViewModel = new QuestionGroupsViewModel(this.Survey.ID.ToString(), this.Survey.QuestionGroups);
        }
        
        public static IEnumerable<Survey> GetSurveys()
        {
            return SurveillanceServices.GetSurveys();
        }

        public void Save()
        {
            // Restore items from viewmodel

            var questionGroups = new QuestionGroups();

            foreach (var qgvm in QuestionGroupsViewModel)
            {
                questionGroups.Add(qgvm.Number, qgvm.QuestionGroup);    
            }
            
            this.Survey.QuestionGroups = questionGroups;

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

    public class QuestionGroupViewModel : ViewModelBase
    {
        public string SurveyId { get; set; }
        public int Number { get; set; }
        public QuestionGroup QuestionGroup { get; set; }

        public QuestionGroupViewModel()
        {
            this.QuestionGroup = new QuestionGroup();
        }

        public QuestionGroupViewModel(QuestionGroup questionGroup)
        {
            this.QuestionGroup = questionGroup;
            this.Number = questionGroup.Number;
        }
    }
}