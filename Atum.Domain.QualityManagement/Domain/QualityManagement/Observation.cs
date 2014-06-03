using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement
{
    public class Observation : Atum.Domain.SurveyManagement.Response
    {
        private Common.Person person;

        public Observation(Common.Person person, string observationTarget)
            : base(new Question(observationTarget,QuestionType.SelectOne),new ResponseChoice(""))
        {
            // TODO: Complete member initialization
            this.person = person;
        }
    }
}
