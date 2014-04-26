using System.ComponentModel;
using System.Linq;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    //public class SurveillanceViewModel
    //{
        
    //}

    public class SurveysViewModel
    {
        public Surveys Surveys{ get; set; }
    }

    public class TracerViewModel    
    {
        //public SurveillanceViewModel SurveillanceModel { get; set; }
        public Facility Facility { get; set; }
        public int SurveyId { get; set; }
        private Survey Survey { get; set; }

        public TracerViewModel()
        {
            // Default ctor for saving
        }

        public TracerViewModel(Survey survey)
        {
            this.Survey = survey;
            this.SurveyId = Convert.ToInt32(survey.ID);
            this.QuestionGroups = new QuestionGroupsViewModel(survey.ID.ToString(), survey.QuestionGroups);
            this.SurveyDate = DateTime.Today.ToShortDateString();
        }

        [Required]
        [Display(Name = "Hospital")]
        public int FacilityId { get; set; }
        public IEnumerable<Facility> Facilities { get; set; }

        [Required]
        [Display(Name = "Building")]
        public int BuildingId { get; set; }
        public IEnumerable<Building> Buildings { get; set; }

        //FloorNumber
        [Required]
        [Display(Name = "Floor")]
        public int FloorNumber { get; set; }

        [Required]
        [Display(Name = "Area/Unit")]
        public int AreaId { get; set; }
        public IEnumerable<Area> Areas { get; set; }

        [Required]
        [Display(Name = "Tracer Type")]
        public int TracerTypeId { get; set; }
        public IEnumerable<TracerType> TracerTypes { get; set; }

        [Required]
        [Display(Name = "Staff Surveyed")]
        public string StaffSurveyed { get; set; }

        [Required]
        [Display(Name="Surveyor")]
        public int SurveyorId { get; set; }
        public IEnumerable<Person> Surveyors { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string SurveyDate { get; set; }

        // [Required] AS - Are notes required?
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public IEnumerable<Department> Departments { get; set; }

        [Required]
        [Display(Name = "Survey Type")]
        public int SurveyTypeId { get; set; }
        public List<SurveyType> SurveyTypes { get; set; }
        
        public QuestionGroupsViewModel QuestionGroups { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public int SelectedResponse { get; set; }

        [Range(1,999, ErrorMessage = "Please choice a response.")]
        public ResponseViewModel[] Responses { get; set; }
    }

    public class ResponseViewModel
    {
        public int ResponseId { get; set; }
        public int NextQuestionId { get; set; }
        public string ResponseDisplayText { get; set; }

        public ResponseViewModel(object value)
        {
            this.ResponseId = Convert.ToInt32(value);
        }

        public static implicit operator ResponseViewModel(string value)
        {
            return new ResponseViewModel(value);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Group : List<QuestionViewModel>
	{

        public Group(KeyValuePair<int, QuestionGroup> item)
        {
            this.Title = item.Value.Title;
//          this.Title = item.Value.Number.ToString() + item.Value.Title;
            this.AddRange(LoadQuestionModels(item.Value.Questions));
    
        }

        private IEnumerable<QuestionViewModel> LoadQuestionModels(Questions questions)
        {
            return questions.Select(item => new QuestionViewModel(item)).ToList();
        }


        [Display(Name = "Title")]
        public string Title { get; set; }
		
	}
    
    public class QuestionGroupsViewModel : List<QuestionGroupViewModel>
    {
        public QuestionGroups QuestionGroups { get; set; }

        public QuestionGroupsViewModel()
        {
        }

        public QuestionGroupsViewModel(string surveyId, QuestionGroups questionGroups)
        {
            this.SurveyId = surveyId;
            this.QuestionGroups = questionGroups;
            SetGroupValues(questionGroups);
        }

        private void SetGroupValues(QuestionGroups questionGroups)
        {
            var i = 0;
            foreach (var item in questionGroups)
            {
                var group = new QuestionGroupViewModel { Number = item.Key, QuestionGroup = item.Value, SurveyId = this.SurveyId};
                this.Add(group);

                i++;
            };
        }

        public string SurveyId { get; set; }
        
    }

    public class QuestionViewModel
    {
        public Question Question { get; set; }

        public QuestionViewModel(Question question)
        {
            Question = question;
            Choices = question.ResponseChoices;
        }

        public string Text { get; set; }
        public int Number { get; set; }

        public List<ResponseChoice> Choices { get; set; }
    }

    public class TracerType { }
}