using System.Linq;
using System.Xml.Serialization;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.QualityManagement;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SurveyWeb.Models
{
    public class SurveysViewModel
    {
        public Surveys Surveys { get; set; }
        public Dictionary<int, Surveys> SurveysByDate { get; internal set; }

        internal Surveys GetOrAddSurveysByDate(int groupIndex)
        {
            var eventSurveys = new Surveys();
            if (!SurveysByDate.ContainsKey(groupIndex))
            {
                SurveysByDate.Add(groupIndex, eventSurveys);
            }
            else
            {
                eventSurveys = SurveysByDate[groupIndex];
            }

            return eventSurveys;
        }
    }

    // TODO: Split view model from data
    public class TracerViewModel    
    {
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
            this.SurveyDate = DateTime.Now;
            this.SurveyTypeId = (int)survey.SurveyType;
            this.SurveyTitle = survey.Title;
        }
        
        [Display(Name = "Title")]
        public string SurveyTitle { get; set; }

        [Required]
        [Display(Name = "Hospital")]
        public int FacilityId { get; set; }

        [XmlIgnore]
        public IEnumerable<Facility> Facilities { get; set; }

        [Required]
        [Display(Name = "Building")]
        public int BuildingId { get; set; }

        [XmlIgnore]
        public IEnumerable<Building> Buildings { get; set; }

        //FloorNumber
        [Required]
        [Display(Name = "Floor")]
        public int FloorNumber { get; set; }

        [Required]
        [Display(Name = "Area/Unit")]
        public int AreaId { get; set; }

        [XmlIgnore]
        public IEnumerable<Area> Areas { get; set; }

        [Required]
        [Display(Name = "Tracer Type")]
        public int TracerTypeId { get; set; }

        [XmlIgnore]
        public IEnumerable<TracerType> TracerTypes { get; set; }

        //[Required]
        [Display(Name = "Staff Surveyed")]
        public string StaffSurveyed { get; set; }

        [Required]
        [Display(Name="Surveyor")]
        public Guid SurveyorId { get; set; }

        [XmlIgnore]
        public IEnumerable<Person> Surveyors { get; set; }

        [Required]
        [Display(Name = "Date")]        
        public DateTime SurveyDate { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime UpdatedDate { get; set; }

        [Display(Name = "Completed Date")]
        public DateTime CompletedDate { get; set; }
        
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [XmlIgnore]
        public IEnumerable<Department> Departments { get; set; }

        [Required]
        [Display(Name = "Survey Type")]
        public int SurveyTypeId { get; set; }

        [XmlIgnore]
        public List<SurveyType> SurveyTypes { get; set; }

        [XmlIgnore]
        public QuestionGroupsViewModel QuestionGroups { get; set; }

        public string ResponseId { get; set; }
        
        [Range(1,999, ErrorMessage = "Please choice a response.")]
        public ResponseViewModel[] Responses { get; set; }
    }

    public class ResponseViewModel
    {
        public Response Response { get; set; }
        public int ResponseId { get; set; }
        public int NextQuestionId { get; set; }
        public string ResponseDisplayText { get; set; }

        public ResponseViewModel()
        {
        }

        public ResponseViewModel(Response response)
        {
            this.Response = response;
        }

        public override string ToString()
        {
            return this.ResponseId.ToString();
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
            foreach (var item in questionGroups)
            {
                var group = new QuestionGroupViewModel { Number = item.Key, QuestionGroup = item.Value, SurveyId = this.SurveyId};
                this.Add(group);
            }
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