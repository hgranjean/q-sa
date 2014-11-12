using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement
{
    /// <summary>
    /// An Observation may is a response to a Question
    /// The Question always relates to an Element of Performance and is an assertion 
    /// that the Element is in compliance per the Guideline or Standard
    /// Hence the Observation always relates to Question/Standard found to be 
    /// out of complicance and contains evidence in the form 
    /// of Text or Files - i.e. Documents or Photographs
    /// </summary>
    public class Observation : Response
    {
        public Person Person { get; private set; }
       
        public Observation(Person person, string observationTarget,string referenceElementKey)
            : base(new Question(observationTarget, QuestionType.SelectOne, null, referenceElementKey), new ResponseChoice(""))
        {
            this.Person = person;
        }

        public Observation(Person person, Question question, ResponseChoice answer) : base(question, answer)
        {
            this.Person = person;
        }

        public FollowUp FollowUp { get; set; }
        public List<string> EvidenceFileInfos { get; set; }

        
    }
}
