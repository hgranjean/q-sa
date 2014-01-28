using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Atum.Domain.Surveillance;
using Atum.Domain.Common;
using Atum.Domain.Surveillance.Specifications;
using Atum.Domain.Basis;

namespace Atum.Domain.Test
{
    [TestClass]
    public class SurveyTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Default Survey Type if empty contructor
            //Reconsider empty constructor
            Surveillance.Survey survey = new Surveillance.Survey();

            ////Set Survey Type - Overwrite Survey Type
            survey.SurveyType = SurveyType.Audit;


            //Survey Basis Document (assert that we can see the TOCElements
            SurveyBasis surveyBasis = new SurveyBasis();
            surveyBasis.TableOfContents = loadTableOContents();

            //Create/Add Survey Questions in default QuestionGroup
            QuestionGroup qGroup = survey.AddQuestionGroup();

            //Create New QuestionGroup
            string groupTittle = "My Group Title";
            QuestionGroup qGroupNew = survey.AddQuestionGroup(groupTittle);


            //Create/Add Survey Questions in new QuestionGroup
            string questionText = "My Question Text";
            QuestionType qType = QuestionType.OpenText;
            Question question = qGroup.AddQuestion(questionText, qType);

            QuestionSpecification questionSpecification = new QuestionSpecification();
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //TOC Element will be displated as hyperlink
            question.BasisReference = surveyBasis.TableOfContents.GetElementByTitle("Element Title");



            //Excercising the question types
            //Add a question of each type and assert specification.
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));


            //Validating a Survey - What is a valid survey: SurveySpecification
            //Yes No
            qType = QuestionType.YesNo;
            questionText = "My YesNo Question Text";
            question = qGroup.AddQuestion(questionText, qType);
            string choiceText = "";
            ResponseChoice choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));


            //Validating a Survey - What is a valid survey: SurveySpecification
            //True or False
            qType = QuestionType.TrueFalse;
            questionText = "My TrueFalse Question Text";
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //Multiple Choice Choose One
            questionText = "My SelectOne Question Text";
            qType = QuestionType.SelectOne;
            question = qGroup.AddQuestion(questionText, qType);
            //First Choice
            choiceText = "SelectOneChoice1";
            choice = question.AddChoice(choiceText);
            //Second Choice
            choiceText = "SelectOneChoice2";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //Multiple Choice Choose Any/Many
            questionText = "My SelectMultiple Question Text";
            qType = QuestionType.SelectMultiple;
            question = qGroup.AddQuestion(questionText, qType);
            //First Choice
            choiceText = "SelectMultipleChoice1";
            choice = question.AddChoice(choiceText);
            //Second Choice
            choiceText = "SelectMultipleChoice2";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //Yes or No - if Yes explain or if No explain
            questionText = "My YesNoConditionalOpen Question Text";
            qType = QuestionType.YesNoConditional;
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "YesNoConditionalChoice";
            ResponseChoice conditionOnChoice = question.GetResponseByText("Yes");
            choice = question.AddConditionalChoice(choiceText, conditionOnChoice);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //Yes or No - if Yes explain or if No explain
            questionText = "My SelectOneConditionalOpen Question Text";
            qType = QuestionType.SelectOneConditional;
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "SelectOneConditionalOpenChoice";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            questionText = "My OpentText Question Text";
            qType = QuestionType.OpenText;
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "OpentTextChoice1";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            questionText = "My OpentVariant Question Text";
            qType = QuestionType.OpenVariant;
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "OpentVariant";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            questionText = "My Ranking Question Text";
            qType = QuestionType.Ranking;
            question = qGroup.AddQuestion(questionText, qType);
            choiceText = "RankingChoice1";
            choice = question.AddChoice(choiceText);
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //Responding to a Survey

        }

        private TableOfContents loadTableOContents()
        {
            TableOfContents retVal = new TableOfContents();
            string elementTitle = "Element Title";
            TOCElement element = retVal.AddElement(elementTitle);


            return retVal;
        }
    }
}
