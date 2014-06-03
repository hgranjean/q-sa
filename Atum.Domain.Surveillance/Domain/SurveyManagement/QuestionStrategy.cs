using System;

namespace Atum.Domain.SurveyManagement
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
