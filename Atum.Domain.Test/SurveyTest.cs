using System;
using System.IO;
using Atum.Utility.XML;
using Atum.Domain.QualityManagement;
using Atum.Domain.Common;
//using Atum.Domain.QualityManagement.Specifications;
using Atum.Domain.Basis;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Atum.Domain.SurveyManagement;
using Atum.Domain.SurveyManagement.Specifications;

namespace Atum.Domain.Test
{
    [TestFixture]
    public class SurveyTest
    {
        [Test]
        public void TestMethod1()
        {
            // Step 1- Initialize Survey

            //Default Survey Type if empty contructor
            //Reconsider empty constructor
            var survey = new Survey();

            survey.AssignNextId(0);
            
            survey.Title = "Survey Template 1";

            ////Set Survey Type - Overwrite Survey Type
            survey.SurveyType = SurveyType.Audit;

            // Step 2 - Initialize TOC

            //Survey Basis Document (assert that we can see the TOCElements
            var surveyBasis = new SurveyBasis();
            surveyBasis.TableOfContents = loadTableOContents();

            //Create/Add Survey Questions in default QuestionGroup
            var qGroup = survey.AddQuestionGroup();

            //Create New QuestionGroup
            var groupTitle = "My Group Title";
            var qGroupNew = survey.AddQuestionGroup(groupTitle); // TODO: herve, QG is added but not used anywhere ??

            //Create/Add Survey Questions in new QuestionGroup
            string questionText = "My Question Text";
            var qType = QuestionType.OpenText;
            var question = qGroup.AddQuestion(questionText, qType);

            var questionSpecification = new QuestionSpecification();
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            //TOC Element will be displated as hyperlink
            question.BasisReference = surveyBasis.TableOfContents.GetElementByTitle("Element Title");
            
            //Excercising the question types
            //Add a question of each type and assert specification.
            Assert.IsTrue(questionSpecification.IsStatisfiedBy(question));

            // Step 3 - Add questions

            //Validating a Survey - What is a valid survey: SurveySpecification
            //Yes No
            qType = QuestionType.YesNo;
            questionText = "My YesNo Question Text";
            question = qGroup.AddQuestion(questionText, qType);
            string choiceText = "";
            var choice = question.AddChoice(choiceText);
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

            var tempFile = Path.GetTempFileName();

            // XmlSerializationUtility.SaveObjectToFile(tempFile, survey);

            //Yes or No - if Yes explain or if No explain
            questionText = "My YesNoConditionalOpen Question Text";
            qType = QuestionType.YesNoConditional;
            question = qGroup.AddQuestion(questionText, qType); // TODO: Should standard choices be added inside AddQuestion or like .AddStandardChoices()?
            question.AddChoice("Yes");
            question.AddChoice("No");
            choiceText = "YesNoConditionalChoice";
            var conditionOnChoice = question.GetResponseByText("Yes");
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

            //Step 4 - Responding to a Survey
        }

        private TableOfContents loadTableOContents()
        {
            var retVal = new TableOfContents();
            string elementTitle = "Element Title";
            var element = retVal.AddElement(elementTitle);
            return retVal;
        }
    }
}
