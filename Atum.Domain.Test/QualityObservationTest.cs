using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;

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
             * Every Observation will be part of a Survey or Surveillance and modeled as a Question within that Survey or (Surveillance)
             * Every Observation/Survey Question Response will be associated with a Standard “Expression”.  
             * Every Observation/Survey Question Response will be associated with a Person/User.  
            */

            //Atum.Domain.Surveillance.Response

            //An Observation implies that that data can be bound to and Expression derived from from a Quality Control Procedure or Standard that can be evaluated to true or false
            //Where the results will indicate compliance (or non-compliance)

            //Such an observations will be modeled as sets of Questions within a Survey or (Surveillance)
            //Observation=>Question, Observation=>Expression, Observation=>Standard
            

            //An observation is performed by a person 

            //The system prior to an Obsevation
            //Survey or Surveillance
            
            //Survey Questions



            //The system after and Observation
            

        }


    }
}
