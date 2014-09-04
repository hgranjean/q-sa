using Atum.Domain.Common;
using Atum.Domain.SurveyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Atum.Domain.QualityManagement
{
    public class Observation : Response
    {
        public Person Person { get; private set; }

        public Observation(Person person, string observationTarget)
            : base(new Question(observationTarget, QuestionType.SelectOne),new ResponseChoice(""))
        {
            this.Person = person;
        }

        public Observation(Person person, Question question, ResponseChoice answer) : base(question, answer)
        {
            this.Person = person;
        }
    }
}
