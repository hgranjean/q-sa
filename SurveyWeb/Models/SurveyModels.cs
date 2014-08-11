using System.Collections.Generic;
using Atum.Domain.QualityManagement;
using SurveyWeb.Services;
using Atum.Domain.SurveyManagement;
using Atum.Domain.Common;

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
            this.Name = survey.Title;//?? "Survey" + survey.ID;
            this.Survey = survey;

            if (this.Survey.QuestionGroups == null)
            {
                this.Survey.AddQuestionGroup("One");
            }

            this.QuestionGroupsViewModel = new QuestionGroupsViewModel(this.Survey.ID.ToString(), this.Survey.QuestionGroups);
        }
        
        public void Save(IPersistenceServices persistenceService)
        {
            // Restore items from viewmodel

            var questionGroups = new QuestionGroups();

            foreach (var qgvm in QuestionGroupsViewModel)
            {
                questionGroups.Add(qgvm.Number, qgvm.QuestionGroup);    
            }
            
            this.Survey.QuestionGroups = questionGroups;

            // [aschmidt]: Move this out of the model as it pertains to the action
                        
            persistenceService.SaveSurvey(this.Survey);
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
            this.AvailableTOCs = new List<KeyValuePair<string, TOCElement>>();
        }

        public QuestionGroupViewModel(QuestionGroup questionGroup)
        {
            this.QuestionGroup = questionGroup;
            this.Number = questionGroup.Number;
            this.AvailableTOCs = new List<KeyValuePair<string, TOCElement>>();
        }

        public IEnumerable<KeyValuePair<string, TOCElement>> AvailableTOCs
        {
            get; set;
        }
    }

    public class ScheduleViewModel : ViewModelBase
    {
        public ScheduleViewModel(object value)
        {
            
        }
    }

    public class SurveyAnalysisViewModel : ViewModelBase
    {
        public int Result { get; set; }

        public SurveyAnalysisViewModel()
        {}

        public SurveyAnalysisViewModel(int result)
        {
            this.Result = result;
        }

        public IEnumerable<RuleApp.SurveyDeliveryRuleApp.EvaluationResult> Followups { get; set; }
    }

    public class CompletedSurveyViewModel
    {
        public IEnumerable<TracerViewModel> CompletedSurveys { get; set; }

        public CompletedSurveyViewModel()
        { }

        public CompletedSurveyViewModel(IEnumerable<TracerViewModel> completedSurveys)
        {
            this.CompletedSurveys = completedSurveys;            
        }
    }
}