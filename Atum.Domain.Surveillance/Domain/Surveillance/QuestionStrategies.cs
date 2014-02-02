using System;
using System.Collections.Generic;
using System.Linq;

namespace Atum.Domain.Surveillance
{
    [Serializable]
    public class QuestionStrategies : List<QuestionStrategy>
    {
        public Question FirstQuestion { get; private set; }

        public new void Add(QuestionStrategy questionStrategy)
        {
            base.Add(questionStrategy);

            if (base.Count == 1)
            {
                SetFirstQuestion(questionStrategy);
            }
        }

        private void SetFirstQuestion(QuestionStrategy questionStrategy)
        {
            Questions qs = questionStrategy.Questions;

            this.FirstQuestion = questionStrategy.Questions.First();
        }
    }
}
