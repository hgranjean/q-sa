using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
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
