using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Models.QualityAuditor
{
    public class AuditViewModels
    {
    }

    
    public class AuditViewModel
    {
        public AuditViewModel()
        {
        }

        public AuditViewModel(Survey survey)
        {
            setQuestions(survey);
        }

        private void setQuestions(Survey survey)
        {
            List<AuditQuestionViewModel> questions = new List<AuditQuestionViewModel>();

            SortedList<int,Question> sortedQuestions = new SortedList<int,Question>();


            foreach (var item in survey.Questions)
            {
                int key = item.Number;

                if (!sortedQuestions.ContainsKey(key))
                {
                    sortedQuestions.Add(key, item);

                }
            }


            foreach (var item in sortedQuestions.Values)
            {
                AuditQuestionViewModel question = new AuditQuestionViewModel();
                question.ResponseChoices = item.ResponseChoices;
                question.Text = item.Text;
                question.Number = item.Number.ToString();
                questions.Add(question);
            };

            Questions = questions;
        }
        public Survey Audit { get; set; }
        public int AuditId { get; set; }
        public string Instructions { get; set; }
        public string Header { get; set; }
        public string Footer { get; set; }
        public IEnumerable<AuditQuestionViewModel> Questions { get; set; }
    }

    /// <summary>
    /// Model for Question View
    /// </summary>
    public class AuditQuestionViewModel
    {
        public ResponseChoices ResponseChoices { get; set; }
        public string Text { get; set; }
        public string Number { get; set; }
    }

    public class AuditResultsViewModel
    {
        public int Score { get; set; }
        public string ScoreMessageTitle { get; set; }
        public string ScoreMessage { get; set; }
        public string ResultIntro { get; set; }
        public string ResultMessage { get; set; }
        public string Disclaimer { get; set; }
    }

    
    public class AuditIntroViewModel
    {
        public int AuditId  { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }



}