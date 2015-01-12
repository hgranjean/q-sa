using Atum.Domain.QualityManagement;
using Atum.Domain.QualityManagement.Auditing;
using Atum.Domain.SurveyManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Atum.Domain.Test
{
    [TestClass]
    public class SurveillanceWorkflowTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            //Manage Surveillance Workflow Tasks
            //A Task is a set of "Questions" related to a Surveillance
            //A Surveillance may have a set of required Question to be answered (or responded to)
            //A Response is an Observation and hence implies a Question
            //A Task Owner makes an Observation - e.g. Joe walked through a security door behind someone without badging in (or using his keycard)
            // i.e. Joe "Tailgated" - Using Nuclear industry terms
            // In order to fulfill a Surveillance requirement, Joe makes an observation.

            // Static or Dynamic Surveillance 

            // Performing an Audit vs a Surveillance


            //Create a Surveillance
            Surveillance surveillance = new Surveillance();

            //A Surveillance is a scheduled event to perform a required Audit.
            surveillance.Start = DateTime.Today;
            surveillance.End = DateTime.Today;
            surveillance.Frequency = Atum.Domain.Common.Frequency.Monthly;
            surveillance.CreatedDate = DateTime.Today;
            surveillance.Title = "";

            //Load or Create a Template
            surveillance.Template = new Survey();




            //Joe is assigned a task - A Surveillance Task
            QualityManagement.SurveillanceTask surveyTask = new QualityManagement.SurveillanceTask();
            surveyTask.Start = DateTime.Today;
            surveyTask.End = DateTime.Today;
            // surveyTask.CreatedDate = DateTime.Today;
            surveyTask.Title = "";


            //Assign the entire Surveillance
            QualityManager qualityManager = new QualityManager();
            

            //Joe views his Assigned Surveillance Tasks
            





            //Joe starts his Assigned Surveillance Tasks

            
            
            
            
            //Joe Completes His Surveillance Tasks


            
            
            
            //Assign One or More Categories applicable to Surveillance

            
            
            
            //Assign along some other Dimension of Surveilance








        }
    }
}
