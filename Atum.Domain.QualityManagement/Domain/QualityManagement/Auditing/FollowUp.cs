using Atum.Domain.Basis;
using Atum.Domain.Common;
using Atum.Domain.QualityManagement;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Domain.QualityManagement.Auditing
{
    /// <summary>
    /// 
    /// </summary>
    public class FollowUp //: Event//DomainObject
    {
        public FollowUp()
        {
        }
       
        public FollowUp(Observation observation, Person assignedTo)
        {
            this.Observation = observation;
            this.ResponsibleParty = assignedTo;
        }


        public Guid Id { get; set; }
        public string Status { get; set; }
        public DateTime InitialDueDate { get; set; }
        public Question Question { get; set; }
        public Observation Observation { get; set; }
        public Person ResponsibleParty { get; set; }

        //Times Sent: 3        
        public int TimeSent { get; set; }
        public DateTime LastSent { get; set; }
        //internal Audit Audit { get; set; }
        public string AuditId { get; set; }
        //public DateTime InspectionDate { get; set; }        

        public Person InspectedBy { get; set; }
        
        //Category: Patient Safety 
        public string Category { get; set; }
        //Item Inspected: Clutter (0735)
        public string ItemInspected { get; set; }
        //Area: 2 North (027)
        public Area Area { get; set; }

        //Responsibility:  Vicki Munson
        //Service: Area (001)
        //Score: Non Compliant
        public string Score { get; set; }

        //Estimated Completion Date:
        public DateTime EstimatedCompletionDate { get; set; }

        //Item Detail: Issue Details:
        public string ItemDetails { get; set; }
        
        //History
        public List<Event> History { get; set; }
   

    }
}
