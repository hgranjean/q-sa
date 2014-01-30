using Atum.Domain.Specification;
using System;

namespace Atum.Domain.Surveillance.Specifications
{
    [Serializable]
    public class QuestionSpecification : ISpecification
    {
        public bool IsStatisfiedBy(Basis.DomainObject domainObject)
        {
            bool retVal = false;
            try
            {
                retVal = true;
                Question question = (Question)domainObject;
                retVal = (question.Text.Length > 0);
                retVal &= validForType(question);


            }
            catch (System.Exception)
            {
                throw;
            }

            return retVal;
        }

        private bool validForType(Question question)
        {
            bool retVal = true;

            switch (question.QuestionType)
            {
                case QuestionType.YesNo:
                    retVal = (question.ResponseChoices.Count == 2);
                    retVal &= (question.ResponseChoices[0].Text == "Yes");
                    retVal &= (question.ResponseChoices[1].Text == "No");
                    break;
                case QuestionType.TrueFalse:
                    retVal = (question.ResponseChoices.Count == 2);
                    retVal &= (question.ResponseChoices[0].Text == "True");
                    retVal &= (question.ResponseChoices[1].Text == "False");
                    break;
                case QuestionType.SelectOne:
                    retVal = (question.ResponseChoices.Count > 1);
                    break;
                case QuestionType.SelectMultiple:
                    retVal = (question.ResponseChoices.Count > 1);
                    break;
                case QuestionType.SelectOneConditional:
                    retVal = (question.ResponseChoices.Count > 1);
                    break;
                case QuestionType.YesNoConditional:
                    retVal = (question.ResponseChoices.Count == 3);
                    retVal &= (question.ResponseChoices[0].Text == "Yes");
                    retVal &= (question.ResponseChoices[1].Text == "No");
                    break;
                case QuestionType.TrueFalseConditional:
                    retVal = (question.ResponseChoices.Count == 3);
                    retVal &= (question.ResponseChoices[0].Text == "True");
                    retVal &= (question.ResponseChoices[1].Text == "False");
                    break;
                case QuestionType.OpenText:
                    //retVal = (question.ResponseChoices.Count == 0);
                    break;
                case QuestionType.OpenVariant:
                    break;
                case QuestionType.Ranking:
                    retVal = (question.ResponseChoices.Count > 1);
                    break;
                default:
                    break;
            }

            return retVal;
        }



    }
}
