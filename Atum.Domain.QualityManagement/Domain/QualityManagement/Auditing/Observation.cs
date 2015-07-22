using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement.Auditing
{
    /// <summary>
    /// An Observation may is a response to a Question
    /// The Question always relates to an Element of Performance and is an assertion 
    /// that the Element is in compliance per the Guideline or Standard
    /// Hence the Observation always relates to Question/Standard found to be 
    /// out of complicance and contains evidence in the form 
    /// of Text or Files - i.e. Documents or Photographs
    /// </summary>
    public class Observation //: Response
    {
       
        public Observation(Person person, string observationTarget,string referenceElementKey)
            //: base(new Question(observationTarget, QuestionType.SelectOne, null, referenceElementKey), new ResponseChoice(""))
        {
            this.Observer = person;
            this.ReferenceElementKey = referenceElementKey;
        }

        //public Observation(Person person, Question question, ResponseChoice answer) 
        //    //: base(question, answer)
        //{
        //    this.Observer = person;
        //}

        public string Remarks { get; set; }
        public string ReferenceElementKey { get; set; }
        public Person Observer { get; set; }
        public DateTime DateObserved { get; set; }
        public string Location { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
        public string Area { get; set; }


        //public Guid FollowUpId { get; set; }
        //public FollowUp FollowUp { get; set; }
        public List<string> EvidenceFileInfos { get; set; }

    }
}
