using System.Linq;
using System.Xml.Serialization;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.QualityManagement;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Atum.Domain;

namespace SurveyWeb.Models
{
    public class SurveysViewModel
    {
        public Surveys Surveys { get; set; }
        public Dictionary<int, List<Tuple<EventUser,Survey>>> SurveysByDate { get; internal set; }

        internal List<Tuple<EventUser,Survey>> GetOrAddSurveysByDate(int groupIndex)
        {
            var eventSurveys = new List<Tuple<EventUser,Survey>>();
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
        public Facility Facility
        {
            get
            {
                if (this.Facilities == null)
                    return default(Facility);
                
                return this.Facilities.FirstOrDefault(m => m.Id == FacilityId);
            }
        }

        public Person Surveyor
        {
            get
            {
                if (this.Surveyors == null)                
                    return default(Person);                
                
                var surveyorId = SurveyorId.ToString("d");
                return this.Surveyors.FirstOrDefault(m => m.Id == surveyorId);
            }
        }
                    
        public int SurveyId { get; set; }
        private Survey _survey { get; set; }

        public TracerViewModel()
        {            
        }

        public TracerViewModel(Survey survey)
        {
            this._survey = survey;
            this.SurveyId =(int)survey.Id;
            this.QuestionGroups = new QuestionGroupsViewModel((int)survey.Id, survey.QuestionGroups);
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

    //TODO: Move this to Survey Models?
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

        public QuestionGroupsViewModel(int surveyId, QuestionGroups questionGroups)
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

        public int SurveyId { get; set; }
    }

    public class QuestionViewModel
    {
        public Question Question { get; set; }

        public QuestionViewModel()
        {
            //Question = new Question();
        }

        public QuestionViewModel(Question question)
        {
            Question = question;
            Choices = question.ResponseChoices;        
        }

        public string Text { get { return this.Question.Text; } set { this.Question.Text = value;} }
        public int Number { get { return this.Question.Number; } set { this.Question.Number = value; } }

        public int QuestionGroupNumber { get; set; }
                
        public QuestionType QuestionType { get { return this.Question.QuestionType; } set { this.Question.QuestionType = value; } }

        public string TOCReference { get { return this.Question.TOCReference; } set { this.Question.TOCReference = value; } }

        public List<ResponseChoice> Choices { get; set; }

        public IEnumerable<KeyValuePair<string, TOCElement>> AvailableTOCs { get; set; }

        public int SurveyId { get; set; }
    }

    public class TracerType { }



    //Follow-Ups
    public class FollowUpViewModel
    {
        //Follow-up ID:
        [Display(Name = "Follow-up ID")]
        public int FollowUpId { get; set; }

        //Times Sent: 3        
        [Display(Name = "Times Sent")]
        public int TimeSent { get; set; }

        //Last Sent: 04/25/2012
        [Display(Name = "Last Sent")]
        public DateTime LastSent { get; set; }

        //Survey: March 2012
        [Display(Name = "Survey")]
        public string SurveillanceId { get; set; }

        //Inspected: 03/20/2012  
        [Display(Name = "Inspected")]
        public DateTime InspectionDate { get; set; }        

        //By: Michelle Kadoun
        [Display(Name = "Inspected By")]
        public string InspectedBy { get; set; }
        
        //Category: Patient Safety 
        [Display(Name = "Category")]
        public string Category { get; set; }

        //Item Inspected: Clutter (0735)
        [Display(Name = "Item Inspected")]
        public string ItemInspected { get; set; }

        //PFA Submitted:  04/12/2012
        
        //Area: 2 North (027)
        [Display(Name = "Area")]
        public Area Area { get; set; }

        //Responsibility:  Vicki Munson
        [Display(Name = "Responsibility")]
        public Person ResponsibleParty { get; set; }
        
        //Service: Area (001)
        
        //Score: Non Compliant
        [Display(Name = "Score")]
        public string Score { get; set; }

        //Estimated Completion Date:
        [Display(Name = "Estimated Completion Date")]
        public DateTime EstimatedCompletionDate { get; set; }

        //Item Detail: Issue Details:
        [Display(Name = "Item Detail")]
        public string ItemDetails { get; set; }
        
        //History
        [Display(Name = "History")]
        public List<Event> History { get; set; }
    }

    public class FollowUpsViewModel : List<FollowUpViewModel>
    {
        public string SearchCriteria { get; set; }    
    }

    public class AssignToViewModel
    {
        public string ResponseId { get; set; }
        public string AssignedTo { get; set; }
        public IEnumerable<Person> Surveyors { get; set; }
    }

    public class PhotoViewModel
    {        
        public string SurveyId { get; set; }
        public string QuestionId { get; set; }
        public string FileName { get; set;}
        public bool IsPublished { get; set; }
        public string UserName { get; set;}
        public DateTime CreatedDateTime { get; set;}

        public string ImageData { get; set; }
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

    public class CompletedObservationsViewModel
    {
        public IEnumerable<ObservationViewModel> Observations { get; set; }

        public CompletedObservationsViewModel() { }

        public CompletedObservationsViewModel(IEnumerable<ObservationViewModel> observations) {
            this.Observations = observations;
        }
    }

    public class ObservationViewModel
    {
        public Observation Observation { get; set; }
        public ObservationViewModel() { }

        public ObservationViewModel(Observation observation)
        {
            this.Observation = observation;
        }
    }
}