using System;

namespace Atum.Domain.SurveyManagement
{
    [Serializable]
    public class QuestionStrategy
    {
        public Questions Questions { get; private set; }

        public QuestionStrategy(Questions questions)
        {
            this.Questions = questions;
        }


    }
}
