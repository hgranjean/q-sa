using System;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class QuestionStrategy
    {
        public Questions Questions { get; private set; }

        public QuestionStrategy(Questions questions)
        {
            // TODO: Complete member initialization
            this.Questions = questions;
        }


    }
}
