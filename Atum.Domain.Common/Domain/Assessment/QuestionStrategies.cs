using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Domain.Assessment
{
    public class QuestionStrategies : List<QuestionStrategy>
    {

        public new void Add(QuestionStrategy questionStrategy)
        {
            base.Add(questionStrategy);

            if (base.Count == 1)
            {
                setFirstQuestion(questionStrategy);
            }
        }

        private void setFirstQuestion(QuestionStrategy questionStrategy)
        {
            Questions qs = questionStrategy.Questions;
            
            
            this.FirstQuestion = questionStrategy.Questions[0];
        }

        public Question FirstQuestion { get; set; }
    }
}
