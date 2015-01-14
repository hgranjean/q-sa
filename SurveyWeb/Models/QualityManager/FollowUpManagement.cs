using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Atum.Domain.QualityManagement;
using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;

namespace SurveyWeb.Models.QualityManager.FollowUpManagement
{

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

        //Template: March 2012
        [Display(Name = "Template")]
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


}