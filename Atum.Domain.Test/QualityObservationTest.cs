using Atum.Domain.QualityManagement;
using Atum.Domain.QualityManagement.Auditing;
using Atum.Domain.SurveyManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Atum.Domain.Test
{
    [TestClass]
    public class QualityObservationTest
    {
        
        [TestMethod]
        public void TestObservation()
        {
            /*
             * Observations
             * An Observation is the identification of data can be bound to and expression derived from a Quality Control Procedure or Standard that can be evaluated to true or false.
             * An Observation is collecting or entering data meant to show compliance with a Quality Control Procedure or Standard. Meaning data can be used to evaluate an expression derived from a Quality Control Procedure or Standard to true or false. 
             * A Quality Control Procedure or Standard can be expressed a collection of predicates or affirmations.
            */

            /*
             * Every Observation will be part of a Template or Surveillance and modeled as a Question within that Template or (Surveillance)
             * Every Observation/Template Question Response will be associated with a Standard “Expression”.  
             * Every Observation/Template Question Response will be associated with a Person/User.  
            */

            //Such observations will be modeled as sets of Questions within a Template or (Surveillance)
            //Observation=>Question, Observation=>Expression, Observation=>Standard
            string userObservation = "Dust on surface";
            string violatedGuidelineDescriptorOrId = "EP-123";//or other descriptor

            //An Observation implies that that data can be bound to and Expression derived 
            //from a Quality Control Procedure or Standard that can be evaluated to true or false (not complying to a Standard or Guideline)
            //Where the results will indicate compliance (or non-compliance)

            //Validate Guideline = We need a violated guideline (e.g. Element of Performance)
            Question questionGuidelineViolated = new Question(violatedGuidelineDescriptorOrId, QuestionType.SelectOne,new QuestionGroup(""),violatedGuidelineDescriptorOrId);

            //An observation is an Atum.Domain.Surveillance.Response
            ResponseChoice responseChoiceUserObservation = new ResponseChoice(userObservation);
            Response responseObservation = new Response(questionGuidelineViolated, responseChoiceUserObservation);

            //Surveillance
            //Is there a currently scheduled surveillance 
            SurveillanceSchedule survSched = new SurveillanceSchedule();

            Surveillance surveillance = new Surveillance(new Survey());
            
            

            //An observation is performed by a person 

            //The system prior to an Obsevation
            //Template or Surveillance
            
            //Template Questions



            //The system after and Observation
            

        }

 
    }
}
