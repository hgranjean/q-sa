using Atum.Database.Surveillance.Models;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Surveillance;
using Atum.Utility.XML;
using SurveyWeb.Controllers;
using SurveyWeb.Models;
using SurveyWeb.RuleApp;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    [Authorize]
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
            var surveys = SurveyViewModel.GetSurveys();

            // Initialize model with some dummy templates
            /*var model = new SurveysViewModel { Surveys = new Surveys
                        {
                            LoadSurvey("Survey Template 1"),
                            LoadSurvey("Survey Template 2"),
                        }
                };*/

            var model = new SurveysViewModel { Surveys = new Surveys() };
            model.Surveys.AddRange(surveys);
            
            return View(model);
        }

        public ActionResult SurveySchedules()
        {
            var surveys = SurveyViewModel.GetSurveys();

            var model = new SurveysViewModel { Surveys = new Surveys() };
            model.Surveys.AddRange(surveys);

            return Surveys();
        }

        public ActionResult DeleteSurvey(long id)
        {
            var model = SurveyViewModel.GetSurveys().FirstOrDefault(m => m.ID == id);

            return View(model);
        }
        
        [HttpPost]
        public ActionResult DeleteSurvey(SurveyViewModel model)
        {
            PersistenceServices.DeleteSurvey(model.Survey.ID.ToString());

            return RedirectToAction("Surveys");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveyDesign(string id)
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit

            Survey survey = null;
            if (!String.IsNullOrWhiteSpace(id))
            {
                var surveys = SurveyViewModel.GetSurveys();

                survey = surveys.FirstOrDefault(item => item.ID.ToString() == id);
            }
            else
            {
                survey = LoadSurvey("Survey Title 1");
            }
            
            var model = new SurveyViewModel(survey);

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
            var model = LoadTracerViewModel(surveyId);
            
            return View(model);
        }

        public ActionResult CompletedSurveys()
        {
            var availableResponses = PersistenceServices.GetResponses();

            // TODO: Filter out response by user
            // foreach (var response in availableResponses) { PersistenceServices.LoadTracer(response); }

            var model = new CompletedSurveyViewModel(availableResponses);

            return View(model);
        }

        public ActionResult ViewCompletedSurvey(string responseId)
        {
            var availableResponses = PersistenceServices.GetResponses();

            foreach (var response in availableResponses)
            {
                if (response.EndsWith(responseId))
                {
                    var model = PersistenceServices.LoadTracer(responseId);

                    LoadTracerReferenceData(model);

                    LoadSurveyData(model);

                    return View("SurveyDelivery", model);
                }
            }

            throw new KeyNotFoundException("Response was not found - " + responseId);
        }

        private void LoadSurveyData(TracerViewModel model)
        {
            int surveyId = model.SurveyId;

            var tracerModel = LoadTracerViewModel(surveyId);

            model.QuestionGroups = tracerModel.QuestionGroups;
        }

        private TracerViewModel LoadTracerViewModel(int? surveyId)
        {
            Survey survey = LoadSurvey("Survey Template 1");
            if (surveyId.HasValue)
            {
                survey = PersistenceServices.GetSurvey(surveyId.Value);

                foreach (var questionGroup in survey.QuestionGroups)
                {
                    if (questionGroup.Value.Questions != null)
                    {
                        foreach (var question in questionGroup.Value.Questions)
                        {
                            if (question.ResponseChoices == null)
                            {
                                question.ResponseChoices = new ResponseChoices();
                            }

                            question.ResponseChoices.Clear();
                            setQuestionChoices(question);
                        }
                    }
                }
            }
            
            var retVal = new TracerViewModel(survey);
            
            LoadTracerReferenceData(retVal);

            return retVal;
        }

        private void LoadTracerReferenceData(TracerViewModel retVal)
        {
            retVal.Buildings = LoadBuildings();
            retVal.Facilities = LoadFacilities();
            retVal.Areas = LoadAreas();
            retVal.Surveyors = LoadSurveyors();
            retVal.Departments = LoadDepartments();
            retVal.FloorNumber = 3;
        }

        private Survey LoadSurvey(string title)
        {
            var survey = new Survey(title);

            ////Set Survey Type - Overwrite Survey Type
            survey.SurveyType = SurveyType.Audit;

            // Step 2 - Initialize TOC

            //Survey Basis Document (assert that we can see the TOCElements
            var surveyBasis = new SurveyBasis();
            surveyBasis.TableOfContents = LoadTableOContents();

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

        private int _questionChoiceNextId = 0;

        private void setQuestionChoices(Question question)
        {
            question.AddChoice("Compliant").SetIdInternal(_questionChoiceNextId++);
            question.AddChoice("Non Compliant").SetIdInternal(_questionChoiceNextId++);
            question.AddChoice("N/A").SetIdInternal(_questionChoiceNextId++);
            // question.AddChoice("Not Scored"); // AS - Not Valid choice 
            question.AddChoice("Follow-Up Completed").SetIdInternal(_questionChoiceNextId++);
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

        private TableOfContents LoadTableOContents()
        {
            var toc = new TableOfContents();
            toc.AddElement("Element Title");
            return toc;
        }

        private IEnumerable<Department> LoadDepartments()
        {
            yield return new Department("Department1", 1);
            yield return new Department("Department2", 2);
        }

        private IEnumerable<Person> LoadSurveyors()
        {
            yield return new Person { FirstName = "Joe", MiddleName = "D", LastName = "Surveyor" };
            yield return new Person { FirstName = "Henry", MiddleName = "M", LastName = "TracerDude" };
        }

        private IEnumerable<Area> LoadAreas()
        {
            yield return new Area("Area1", 1);
            yield return new Area("Area2", 2);
        }

        private IEnumerable<Facility> LoadFacilities()
        {
            // var _dbContext = new AtumSurveillanceContext();

            // return _dbContext.Hospitals.Select(hospital => new Facility(hospital.Name, Int32.Parse(hospital.Id)));

            yield return new Facility("Facility1", 1);
            yield return new Facility("Facility2", 2);
        }

        private IEnumerable<Building> LoadBuildings()
        {
            yield return new Building("Building1", 1);
            yield return new Building("Building2", 2);
        }


        public ActionResult ViewReference(string standardId) 
        {
            TOCElement model = new TOCElement("");
            model = GetViewModel(standardId);


            return View(model);            
        }

        public TOCElement GetViewModel(string Id)
        {

            var model = new TOCElement(Id);

            if (Id=="LS.02.01.20 EP27")
            {
                model.Content = LoadContent(Id);
            }

            if (Id == "LS.04.03.02")
            {   
                string appPath = AppDomain.CurrentDomain.RelativeSearchPath;

                appPath = appPath + @"\\..\RuleApp\";

                model = (TOCElement)XmlSerializationUtility.GetObjectFromFile(appPath + @"Standards\"+Id+".xml", typeof(TOCElement));
            }

            return model;
        }

        public IEnumerable GetTOCs()
        {
            yield return new KeyValuePair<string, TOCElement>("", TOCElement.None);
            yield return new KeyValuePair<string, TOCElement>("LS.02.01.20 EP27", GetViewModel("LS.02.01.20 EP27"));
            yield return new KeyValuePair<string, TOCElement>("LS.04.03.02", GetViewModel("LS.04.03.02"));
        }

        private string[] LoadContent(string Id)
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

        [HttpPost]
        public ActionResult SaveSurvey(TracerViewModel viewModel, FormCollection values)
        {
            var survey = PersistenceServices.GetSurvey(viewModel.SurveyId);
            
            var questions = new Questions();
            var responses = new Responses();

            int qIndex = 0;
            var responsesViewModels = new ResponseViewModel[100];
            foreach (var questionGroup in survey.QuestionGroups)
            {
                foreach (var question in questionGroup.Value.Questions)
                {
                    questions.Add(question);
                    var value = values.GetValue("Responses[" + qIndex + "]");
                    if (value != null)
                    {
                        var responseId = (int) value.ConvertTo(typeof (int));

                        var response = new Response(question, question.ResponseChoices.FirstOrDefault(r => r.ID == responseId));
                        responses.Add(response);
                        
                        responsesViewModels[qIndex] = new ResponseViewModel(response) { ResponseId = responseId};
                    }
                    qIndex++;
                }
            }
            Array.Resize(ref responsesViewModels, qIndex);

            viewModel.Responses = responsesViewModels;

            var surveyDelivery = new SurveyDeliveryRuleApp();
            surveyDelivery.InitializeRuleApp();
            
            var analysisViewModel = new SurveyAnalysisViewModel();
            var evaluationResults = surveyDelivery.EvaluateSurvey(questions, responses);
            analysisViewModel.Result = evaluationResults.Count;
            
            var result = new List<SurveyDeliveryRuleApp.EvaluationResult>();
            for (int i = 0; i < evaluationResults.Count; i++)
            {
                // TODO: Do this in rules
                if (evaluationResults[i].IsFollowup)
                {
                    evaluationResults[i].TextResult = questions[i].Text;
                    result.Add(evaluationResults[i]);
                }
            }

            analysisViewModel.Followups = result;

            PersistenceServices.SaveTracer(viewModel);
            
            return View("SurveyAnalysis", analysisViewModel);
        }
        
        public ActionResult Create(Survey survey)
        {
            var viewModel = new SurveyViewModel(survey);

            viewModel.Save();

            return View(viewModel);
        }

        public ActionResult CreateQuestionGroup(long surveyId)
        {
            Survey survey = PersistenceServices.GetSurveys().First(item => item.ID == surveyId);

            return View(new SurveyViewModel(survey));
        }

        [HttpPost]
        public ActionResult CreateQuestionGroup(SurveyViewModel viewModel)
        {
            var survey = PersistenceServices.GetSurvey(Convert.ToInt32(viewModel.Survey.ID));

            if (survey.QuestionGroups == null)
            {
                survey.QuestionGroups = new QuestionGroups();
            }
            int newGroupIndex = survey.QuestionGroups.Count() + 1;

            survey.AddQuestionGroup("New Group " + newGroupIndex);

            viewModel = new SurveyViewModel(survey);

            viewModel.Save();

            return View("SurveyDesign", viewModel);
        }

        [HttpPost]
        public ActionResult Save(SurveyViewModel viewModel)
        {
            viewModel.Save();

            return View();
        }

        public ActionResult EditQuestionGroup(string surveyId, string groupId)
        {
            var survey = PersistenceServices.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            return View(new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
                {
                    SurveyId = surveyId,
                    Number = int.Parse(groupId)
                });
        }

        [HttpPost]
        public ActionResult EditQuestionGroup(QuestionGroupViewModel viewModel)
        {
            var survey = PersistenceServices.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {   
                survey.QuestionGroups[viewModel.Number] = viewModel.QuestionGroup;
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save();

            return View("SurveyDesign", surveyViewModel);
        }

        public ActionResult DeleteQuestionGroup(string surveyId, string groupId)
        {
            var survey = PersistenceServices.GetSurveys().First(item => item.ID.ToString() == surveyId);

            ViewBag.QuestionGroupId = groupId;

            return View(new QuestionGroupViewModel(survey.QuestionGroups[int.Parse(groupId)])
                {
                    SurveyId = surveyId,
                    Number = int.Parse(groupId)
                });
        }

        [HttpPost]
        public ActionResult DeleteQuestionGroup(QuestionGroupViewModel viewModel)
        {
            var survey = PersistenceServices.GetSurveys().First(item => item.ID.ToString() == viewModel.SurveyId);

            if (viewModel.QuestionGroup != null && viewModel.QuestionGroup.Questions != null)
            {
                var questionGroupToDelete = survey.QuestionGroups[viewModel.Number];
                survey.QuestionGroups.Remove(questionGroupToDelete.Number);
            }

            var surveyViewModel = new SurveyViewModel(survey);

            surveyViewModel.Save();

            return View("SurveyDesign", surveyViewModel);
        }

        public ActionResult AddQuestion(string surveyId, string questionGroupId)
        {
            var survey = PersistenceServices.GetSurvey(Convert.ToInt32(surveyId));

            var questionGroup = survey.QuestionGroups[Convert.ToInt32(questionGroupId)];

            questionGroup.AddQuestion(string.Empty, QuestionType.SelectOne);

            new SurveyViewModel(survey).Save();

            return View("EditQuestionGroup", new QuestionGroupViewModel(questionGroup){SurveyId = surveyId});
        }

        public ActionResult EditNotes(string questionId)
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public JsonResult GetEvents(double? start, double? end)
        {
            // var fromDate = ConvertFromUnixTimestamp(start);
            // var toDate = ConvertFromUnixTimestamp(end);

            // var rep = Resolver.Resolve<IEventRepository>();
            // var events = rep.ListEventsForUser(userName, fromDate, toDate);

            /*var eventList = new[]{ new
                {
                    id = "1",
                    title = "Click for google",
                    url = "http://google.com/",
                    start = DateTime.Today.ToString("s"),
                    end = DateTime.Today.AddDays(1).ToString("s"),
                    allDay = false
                }};*/

            var eventList = new[] {
                    new
                        {
                            id = 999,
                            title = "All Day Event",
                            start = "2014-05-20",
                            end = "2014-05-20"
                        },
                    new
                        {
                            id = 999,
                            title = "Long Event",
                            start = "2014-05-27",
                            end = "2014-06-10"
                        },
                    new
                        {
                            id = 999,
                            title = "Repeating Event",
                            start = "2014-06-09T16:00:00",
                            end = "2014-06-09T16:00:00"
                        },
                    new
                        {
                            id = 999,
                            title = "Repeating Event",
                            start = "2014-06-16T16:00:00",
                            end = "2014-06-16T16:00:00"
                        },
                    new
                        {
                            id = 999,
                            title = "Meeting",
                            start = "2014-06-12T10:30:00",
                            end = "2014-06-12T12:30:00"
                        },
                    new
                        {
                            id = 999,
                            title = "Lunch",
                            start = "2014-06-12T12:00:00",
                            end = "2014-06-12T12:00:00",
                        },
                    new
                        {
                            id = 999,
                            title = "Birthday Party",
                            start = "2014-06-13T07:00:00",
                            end = "2014-06-13T07:00:00"
                        },
                    new
                        {
                            id = 99,
                            title = "Click for Google",
                            start = "2014-05-28",
                            end = "2014-05-28"
                        }
                };

            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditEvent(int id)
        {
            return View();
        }
    }
}