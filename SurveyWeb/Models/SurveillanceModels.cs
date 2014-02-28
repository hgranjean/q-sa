using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Surveillance;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        private Atum.Domain.Surveillance.Survey Survey { get; set; }

        public TracerViewModel(Atum.Domain.Surveillance.Survey survey)
        {
            this.Survey = survey;
            this.QuestionGroups = new QuestionGroupsViewModel(survey.QuestionGroups);
        }

        [Required]
        [Display(Name = "Hospital")]
        public int FacilityId { get; set; }
        public List<Facility> Facilities { get; set; }

        [Required]
        [Display(Name = "Building")]
        public int BuildingId { get; set; }
        public List<Building> Buildings { get; set; }

        //FloorNumber
        [Required]
        [Display(Name = "Floor")]
        public int FloorNumber { get; set; }

        [Required]
        [Display(Name = "Area/Unit")]
        public int AreaId { get; set; }
        public List<Area> Areas { get; set; }

        [Required]
        [Display(Name = "Tracer Type")]
        public int TracerTypeId { get; set; }
        public List<TracerType> TracerTypes{ get; set; }

        [Required]
        [Display(Name = "Staff Surveyed")]
        public string StaffSurveyed { get; set; }

        [Required]
        [Display(Name="Surveyor")]
        public int SurveyorId { get; set; }
        public List<Person> Surveyors { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string SurveyDate { get; set; }

        [Required]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public List<Department> Departments { get; set; }

        [Required]
        [Display(Name = "Survey Type")]
        public int SurveyTypeId { get; set; }
        public List<SurveyType> SurveyTypes { get; set; }

        

        public QuestionGroupsViewModel QuestionGroups { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
        public List<string> Responses { get; set; }
        

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
            this.AddRange(loadQuestionModels(item.Value.Questions));
    
        }

        private IEnumerable<QuestionViewModel> loadQuestionModels(Questions questions)
        {
            List<QuestionViewModel> retVal = new List<QuestionViewModel>();
            foreach (var item in questions)
            {
                retVal.Add(new QuestionViewModel(item));
            }
            return retVal;

        }


        [Display(Name = "Title")]
        public string Title { get; set; }
		
	}
    
    public class QuestionGroupsViewModel : List<Group>
    {
        private QuestionGroups questionGroups;

        public QuestionGroupsViewModel(QuestionGroups questionGroups)
        {
            // TODO: Complete member initialization
            this.questionGroups = questionGroups;
            setGroupValues(questionGroups);
        }

        private void setGroupValues(QuestionGroups questionGroups)
        {
            foreach (var item in questionGroups)
            {
                Group group = new Group(item);
                this.Add(group);
            };
        }
        
    }
    
    
    /// <summary>
    /// Question View Model
    /// </summary>
    public class QuestionViewModel
    {

        public QuestionViewModel(Atum.Domain.Surveillance.Question question)
        {
            Question = question;
            Choices = question.ResponseChoices;
        }

        public QuestionViewModel()
        {
            Question = new Question();
        }

        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Required]
        [Display(Name = "Number")]
        public int Number { get; set; }
        [Display(Name = "Label")]
        public string Label { get; set; }
        [Required]
        [Display(Name = "Question Type")]
        public int QuestionTypeId { get; set; }
        public List<QuestionType> QuestionTypes { get; set; }

        public TOCElement BasisReference { get; set; }
        public QuestionType QuestionType { get; set; }

        public List<ResponseChoice> Choices { get; set; }
        public Atum.Domain.Surveillance.Question Question { get; set; }

    }
    public class TracerType { }

    public class FrequencyModel
    {
        public FrequencyModel()
        {
            FrequencyList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> FrequencyList { get; set; }
    }

    public class ScheduleViewModel
    {
        private SurveillanceSchedule surveillanceSchedule;

        public ScheduleViewModel(SurveillanceSchedule surveillanceSchedule)
        {
            // TODO: Complete member initialization
            this.surveillanceSchedule = surveillanceSchedule;
        }
        public Survey Survey { get; set; }
        public DateTime StartDate { get; set; }
        public FrequencyModel Frequency { get; set; }
        [Display(Name = "Frequency")]
        public int FrequencyId { get; set; }

        [Display(Name = "Template")]
        public int SurveyId { get; set; }

        public Surveys Surveys { get; set; }

    }
}