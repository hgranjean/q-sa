using Atum.Domain.Common;
using Atum.Domain.Surveillance;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class SurveillanceController : Controller
    {
        //
        // GET: /Surveillance/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// View List of Surveys
        /// </summary>
        /// <returns></returns>
        public ActionResult Surveys()
        {
            SurveysViewModel model = new SurveysViewModel();
            model.Surveys = new Atum.Domain.Surveillance.Surveys();
            model.Surveys.Add(this.LoadSurvey("Survey Template 1"));
            model.Surveys.Add(this.LoadSurvey("Survey Template 2"));
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDesign()
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit
            SurveyViewModel model = new SurveyViewModel(LoadSurvey("Survey Title 1"));

            return View(model);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public ActionResult SurveyDelivery(int? surveyId)
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit
            TracerViewModel model = LoadTracerViewModel();

            
            return View(model);

        }

        private TracerViewModel LoadTracerViewModel()
        {

            Survey survey = LoadSurvey("Survey Template 1");
            TracerViewModel retVal = new TracerViewModel(survey);
            retVal.Buildings = LoadBuildings();
            retVal.Facilities = LoadFacilities();
            retVal.Areas = LoadAreas();
            retVal.Surveyors = loadSurveyors();
            retVal.Departments = loadDepartments();
            retVal.FloorNumber = 3;

            return retVal;
        }


        private Survey LoadSurvey(string title)
        {
            var survey = new Survey(title);

            ////Set Survey Type - Overwrite Survey Type
            survey.SurveyType = SurveyType.Audit;

            // Step 2 - Initialize TOC

            //Survey Basis Document (assert that we can see the TOCElements
            var surveyBasis = new SurveyBasis();
            surveyBasis.TableOfContents = loadTableOContents();

            Question question = null;
            var qGroup0211 = survey.AddQuestionGroup("0211_Doors ");
            question = qGroup0211.AddQuestion("0211", "No items covering doors, i.e. decorations, paper, etc. ", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0214 = survey.AddQuestionGroup("0214_Adequate Lighting");
            question = qGroup0214.AddQuestion("0214", "Lighting is adequate. ", QuestionType.SelectOne);
            question.BasisReference = new TOCElement("Std: LS.02.01.20 EP27 ");
            setQuestionChoices(question);

            var qGroup0215 = survey.AddQuestionGroup("0215_ Personal Items");
            question = qGroup0215.AddQuestion("0215", "No items stored under the sink in kitchen area.  ", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0218 = survey.AddQuestionGroup("0218_Unoccupied Rooms ");
            question = qGroup0218.AddQuestion("0218", "Unoccupied rooms are locked.", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0219 = survey.AddQuestionGroup("0219_ Violent/Disruptive Behavior");
            question = qGroup0219.AddQuestion("0219", "How do you respond to violent or disruptive behavior?", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0220 = survey.AddQuestionGroup("0220_Weapons");
            question = qGroup0220.AddQuestion("0220", "How do you respond to violent or disruptive behavior with weapons? ", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0221 = survey.AddQuestionGroup("0221_Authorized Identification");
            question = qGroup0221.AddQuestion("0221", "Are all individuals in area wearing their authorized identification according to hospital policy?", QuestionType.SelectOne);
            setQuestionChoices(question);


            var qGroup0222 = survey.AddQuestionGroup("0222_Emergency Numbers Posted");
            question = qGroup0222.AddQuestion("0222", "Emergency numbers are visibly posted. ", QuestionType.SelectOne);
            question.BasisReference = new TOCElement("Std: EC.02.01.01 EP10 ");
            setQuestionChoices(question);

            var qGroup0223 = survey.AddQuestionGroup("0223_Gas Cylinders Secured");
            question = qGroup0223.AddQuestion("0223", "Are gas cylinders properly secured? ", QuestionType.SelectOne);
            question.BasisReference = new TOCElement("Std: EC.02.03.01 EP1");
            setQuestionChoices(question);

            return survey;
        }

        private void setQuestionChoices(Question question)
        {
            question.AddChoice("N/A");
            question.AddChoice("Non Compliant");
            question.AddChoice("Not Scored");
            question.AddChoice("Compliant");
            question.AddChoice("Follow-Up Completed");

        }



        private void setQuestion(QuestionGroup questionGroup, QuestionType questionType)
        {
            Question question = null;
            switch (questionType)
            {
                case QuestionType.YesNo:
                    question = questionGroup.AddQuestion("All vents are clean and free from dust.", questionType);
                    break;
                case QuestionType.TrueFalse:
                    question = questionGroup.AddQuestion("All vents are clean and free from dust.", questionType);
                    break;
                case QuestionType.SelectOne:
                    break;
                case QuestionType.SelectMultiple:
                    break;
                case QuestionType.YesNoConditional:
                    break;
                case QuestionType.TrueFalseConditional:
                    break;
                case QuestionType.SelectOneConditional:
                    break;
                case QuestionType.OpenText:
                    break;
                case QuestionType.OpenVariant:
                    break;
                case QuestionType.Ranking:
                    break;
                default:
                    break;
            };
        }

        private TableOfContents loadTableOContents()
        {
            var retVal = new TableOfContents();
            string elementTitle = "Element Title";
            var element = retVal.AddElement(elementTitle);
            return retVal;
        }

        private List<Department> loadDepartments()
        {
            List<Atum.Domain.Common.Department> retVal = new List<Atum.Domain.Common.Department>();
            Department dept = new Atum.Domain.Common.Department("Department1",1);

            return retVal;
        }

        private List<Person> loadSurveyors()
        {
            List<Atum.Domain.Common.Person> retVal = new List<Atum.Domain.Common.Person>();
            Person person = new Atum.Domain.Common.Person();
            person.FirstName = "Joe";
            person.MiddleName = "D";
            person.LastName = "Surveyor";
            retVal.Add(person);

            person = new Atum.Domain.Common.Person();
            person.FirstName = "Henry";
            person.MiddleName = "M";
            person.LastName = "TracerDude";
            retVal.Add(person); 
            
            return retVal;
        }

        private List<Atum.Domain.Common.Area> LoadAreas()
        {
            List<Atum.Domain.Common.Area> retVal = new List<Atum.Domain.Common.Area>();

            retVal.Add(new Atum.Domain.Common.Area("Area1", 1));
            retVal.Add(new Atum.Domain.Common.Area("Area2", 2));
            return retVal;
        }

        private List<Atum.Domain.Healthcare.Facility> LoadFacilities()
        {
            List<Atum.Domain.Healthcare.Facility> retVal = new List<Atum.Domain.Healthcare.Facility>();

            retVal.Add(new Atum.Domain.Healthcare.Facility("Facility1", 1));
            retVal.Add(new Atum.Domain.Healthcare.Facility("Facility2", 2));
            return retVal;
        }

        private List<Atum.Domain.Common.Building> LoadBuildings()
        {
            List<Atum.Domain.Common.Building> retVal = new List<Atum.Domain.Common.Building>();

            retVal.Add(new Atum.Domain.Common.Building("Building1", 1));
            retVal.Add(new Atum.Domain.Common.Building("Building2", 2));
            return retVal;
        }


        public ActionResult ViewReference(string Id) 
        {
            TOCElement model = new TOCElement("");
            model = getViewModel(Id);


            return View(model);            
        }

        private TOCElement getViewModel(string Id)
        {

            Id = "LS.02.01.20 EP27";
            var model = new TOCElement(Id);

            if (Id=="LS.02.01.20 EP27")
            {
                model.Content = loadContent(Id);
            }
                        
            
            return model;
        }

        private string[] loadContent(string Id)
        {
            List<string> retVal = new List<string>();
            retVal.Add("LS.02.01.20");
            retVal.Add("Elements of Performance for LS.02.01.20");
            retVal.Add("Doors in a means of egress are unlocked in the direction of egress. (For full text and any exceptions, refer to NFPA 101-2000: 18/19.2.2.2.4)");
            retVal.Add("1. Exits discharge to the outside at grade level or through an approved exit passageway that is continuous and terminates at a public way or at an exterior exit discharge. (For full text and any exceptions, refer to NFPA 101-2000: 7.7)");
            retVal.Add("8. In new buildings, exit corridors are at least 8 feet wide; in existing buildings, exit corridors are at least 4 feet wide. If modifying existing buildings with exit corridors that exceed 8 feet, the exit corridors cannot be reduced to less than 8 feet. (For full text and any exceptions, refer to NFPA 101-2000: 18/19.2.3.3)");
            retVal.Add("11. Exits, exit accesses, and exit discharges are clear of obstructions or impediments to the public way, such as clutter (for example, equipment, carts, furniture), construction material, and snow and ice. (For full text and any exceptions, refer to NFPA 101-2000: 7.1.10.1)");
            retVal.Add("13. Resident sleeping rooms open directly onto an exit access corridor. (For full text and any exceptions, refer to NFPA 101-2000: 18/19.2.5.1)");
            retVal.Add("21. Means of egress are adequately illuminated at all points, including angles and intersections of corridors and passageways, stairways, stairway landings, exit doors, and exit discharges. (For full text and any exceptions, refer to NFPA 101-2000: 18/19.2.8)");
            retVal.Add("27. Illumination in the means of egress, including exit discharges, is arranged so that failure of any single light fixture or bulb will not leave the area in darkness. (For full text and any exceptions, refer to NFPA 101-2000: 7.8.1.4)");

            return retVal.ToArray();

        }


        public ActionResult Dashboard()
        {
            return View();
        }
    }
}