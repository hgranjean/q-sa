using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Surveillance
{
    public class QuestionStrategy
    {
        public Questions Questions { get; set; }

        public QuestionStrategy(Surveillance.Questions questions)
        {
            // TODO: Complete member initialization
            this.Questions = questions;
        }


    }
}
