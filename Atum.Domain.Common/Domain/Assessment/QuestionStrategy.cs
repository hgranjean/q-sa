using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    public class QuestionStrategy
    {
        public Questions Questions { get; set; }

        public QuestionStrategy(Assessment.Questions questions)
        {
            // TODO: Complete member initialization
            this.Questions = questions;
        }


    }
}
