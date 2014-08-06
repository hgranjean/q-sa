using System.Data.Entity;
using System.Data.Entity.Validation;
using Atum.Database.Surveillance.Models;
using Atum.Domain;
using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Utility.XML;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SurveyWeb.Models;
using SurveyWeb.RuleApp;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Atum.Utility;
using SurveyWeb.Repository;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class SurveillanceController : Controller
    {        
        private readonly SurveillanceService surveillanceService;
        private readonly SurveyService surveyService;
        private readonly TaskService taskService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPersistenceServices persistenceService;
        
        public SurveillanceController(SurveillanceService surveillanceService,
            TaskService taskService,
            SurveyService surveyService,
            IPersistenceServices persistenceService,
            UserManager<ApplicationUser> userManager)
        {
            
            this.surveillanceService = surveillanceService;
            this.surveyService = surveyService;
            this.taskService = taskService;
            this.userManager = userManager;
            this.persistenceService = persistenceService;
        }

        protected override void Dispose(bool disposing)
        {
            /*if (disposing && userManager != null)
            {
                userManager.Dispose();
                userManager = null;
            }*/
            base.Dispose(disposing);
        }



        /// <summary>
        /// Display a list of Ad-hoc Observations
        /// </summary>
        /// <returns></returns>
        public ActionResult Observations(int? ownerId)
        {
            return View();
        }

        /// <summary>
        /// Update Observation View
        /// </summary>
        /// <param name="observation"></param>
        /// <returns></returns>
        public ActionResult ClassifyObservation(string observation)
        {
            var model = new StandardElementViewModel();
            if (!string.IsNullOrWhiteSpace(observation))
            {
                model.Observation = observation;
                model = ServiceManager.GetService<LearningServices>().Classify(observation);

            }
            return View();
        }


        //
        // GET: /Surveillance/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SurveySchedules()
        {
            var model = GetSurveySchedules(false);

            return View(model);
        }

        private SurveysViewModel GetSurveySchedules(bool isPastDue)
        {            
            var surveys = persistenceService.GetSurveys();            

            var userId = User.Identity.GetUserId();
            
            IEnumerable<EventUser> events = null;
            if (isPastDue)
            {                
                events = taskService.GetPastDueTasks(userId);
            } else {
                events = taskService.GetNextTasks(userId);            
            }         

            var model = new SurveysViewModel { SurveysByDate = new Dictionary<int, Surveys>() };

            foreach (var @event in events)
            {
                var eventSurveys = model.GetOrAddSurveysByDate(@event.Event.Start.ToGroupIndex());

                var survey = surveys.FirstOrDefault(s => s.Guid.ToString() == @event.Event.SurveyId);

                if (survey != default(Survey))
                {
                    eventSurveys.Add(survey);
                }
            }
            return model;
        }
        
        public ActionResult SurveySchedulesPartial()
        {
            var model = GetSurveySchedules(false);

            return PartialView("SurveySchedules", model);
        }

        public ActionResult PastDueSurveys()
        {
            var model = GetSurveySchedules(true);

            return View(model);
        }

        public ActionResult PastDueSurveysPartial()
        {
            var model = GetSurveySchedules(true);

            return PartialView("PastDueSurveys", model);
        }


        public ActionResult EditNotes(string questionId)
        {
            return View();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SurveyDelivery(int id)
        {
            //Using default SurveyType of Surveillance vs Evaluation, Assessment, Audit
            var model = LoadTracerViewModel(id);

            return View(model);
        }

        public ActionResult CompletedSurveys()
        {
            var model = GetCompletedSurveys();

            return View(model);
        }

        public ActionResult CompletedSurveysPartial()
        {
            var model = GetCompletedSurveys();

            return PartialView("CompletedSurveys", model);
        }

        private CompletedSurveyViewModel GetCompletedSurveys()
        {            
            var surveys = persistenceService.GetSurveys();

            // Filtering out responses by user
            var userId = User.Identity.GetUserId();

            var responses = surveillanceService.GetResponses(userId);

            var models = new List<TracerViewModel>();
            foreach (var response in responses)
            {
                var tracerModel = persistenceService.LoadTracer(response);
                LoadTracerReferenceData(tracerModel);
                tracerModel.SurveyTitle = surveys.FirstOrDefault(m => m.ID == tracerModel.SurveyId).Title;
                models.Add(tracerModel);
            }

            var model = new CompletedSurveyViewModel(models);
            return model;
        }

        public ActionResult ViewCompletedSurvey(string id)
        {            
            var availableResponses = persistenceService.GetResponses();

            foreach (var response in availableResponses)
            {
                if (response.Contains(id))
                {   
                    var model = persistenceService.LoadTracer(id);

                    LoadTracerReferenceData(model);

                    LoadSurveyData(model);

                    ViewBag.IsReadOnly = true;

                    return View("SurveyDelivery", model);
                }
            }

            throw new KeyNotFoundException("Response was not found - " + id);
        }

        private void LoadSurveyData(TracerViewModel model)
        {
            int surveyId = model.SurveyId;

            var tracerModel = LoadTracerViewModel(surveyId);

            model.QuestionGroups = tracerModel.QuestionGroups;
        }

        private TracerViewModel LoadTracerViewModel(int? surveyId)
        {
            Survey survey = null; // = LoadSurvey("Survey Template 1");
            if (surveyId.HasValue)
            {                
                survey = persistenceService.GetSurvey(surveyId.Value);

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
                            SetQuestionChoices(question);
                        }
                    }
                }
            }
            
            var retVal = new TracerViewModel(survey);
            
            LoadTracerReferenceData(retVal);

            return retVal;
        }

        internal void LoadTracerReferenceData(TracerViewModel retVal)
        {
            retVal.Buildings = LoadBuildings();
            retVal.Facilities = LoadFacilities();
            retVal.Areas = LoadAreas();
            retVal.Surveyors = LoadSurveyors();
            retVal.Departments = LoadDepartments();
            retVal.FloorNumber = 3;
        }

        private int _questionChoiceNextId = 0;

        private void SetQuestionChoices(Question question)
        {
            question.AddChoice("Compliant").SetIdInternal(_questionChoiceNextId++);
            question.AddChoice("Non Compliant").SetIdInternal(_questionChoiceNextId++);
            question.AddChoice("N/A").SetIdInternal(_questionChoiceNextId++);
            // question.AddChoice("Not Scored"); // AS - Not Valid choice 
            question.AddChoice("Follow-Up Completed").SetIdInternal(_questionChoiceNextId++);
        }

        private void SetQuestion(QuestionGroup questionGroup, QuestionType questionType)
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

        private TableOfContents LoadTableOfContents()
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
            // yield return new Person { FirstName = "Joe", MiddleName = "D", LastName = "Surveyor" };
            // yield return new Person { FirstName = "Henry", MiddleName = "M", LastName = "TracerDude" };
            
            return surveillanceService.GetPersons();
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
            //TODO: Get Reference View Model from Standard Services
            //model = GetViewModel(standardId);


            return View(model);            
        }

        /// <summary>
        /// TODO: Move to Dashboard/Menu Controller
        /// </summary>
        /// <returns></returns>
        public ActionResult Dashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    ViewBag.ShowAdminContent = userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
                    ViewBag.ShowManagerContent = userManager.IsInRole(User.Identity.GetUserId(), "Manager");
                    ViewBag.ShowTeamMemberContent = userManager.IsInRole(User.Identity.GetUserId(), "Team Member");
                }
                catch (Exception ex)
                {
                    Session.Abandon();

                    RedirectToAction("Index", "Home");
                }
            }
            
            return View();
        }


        [HttpPost]
        public ActionResult SaveSurvey(TracerViewModel viewModel, FormCollection values)
        {            
            var survey = persistenceService.GetSurvey(viewModel.SurveyId);

            var questions = new Questions();
            var responses = new Responses();

            int qIndex = 0;
            bool isObservation = false;
            var responsesViewModels = new ResponseViewModel[100];
            foreach (var questionGroup in survey.QuestionGroups)
            {
                foreach (var question in questionGroup.Value.Questions)
                {
                    questions.Add(question);
                    var value = values.GetValue("Responses[" + question.Number /* qIndex  */ + "]");
                    if (value != null)
                    {
                        var responseId = (int)value.ConvertTo(typeof(int));

                        var response = new Response(question, question.ResponseChoices.FirstOrDefault(r => r.ID == responseId));
                        responses.Add(response);

                        responsesViewModels[qIndex] = new ResponseViewModel(response) { ResponseId = responseId };
                    }
                    qIndex++;
                }

                var observationText = values["txtObservation" + questionGroup.Key];
                if (!String.IsNullOrWhiteSpace(observationText))
                {
                    // Add observation
                    var newQuestion = questionGroup.Value.AddQuestion(observationText, QuestionType.SelectOne);
                    var classifyModel = ServiceManager.GetService<LearningServices>().Classify(observationText);
                    newQuestion.TOCReference = classifyModel.StandardId;
                    isObservation = true;
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

            var userId = User.Identity.GetUserId();
            
            var user = surveillanceService.GetUser(userId);

            var responseEntry = surveillanceService.AddResponse(user);            

            viewModel.ResponseId = responseEntry.Id;

            persistenceService.SaveTracer(viewModel);

            if (isObservation)
            {
                var viewModelWithResponses = viewModel;

                viewModel = LoadTracerViewModel(viewModel.SurveyId);

                viewModel.Responses = viewModelWithResponses.Responses;

                return View("SurveyDelivery", viewModel);
            }
            else
            {
                return View("SurveyAnalysis", analysisViewModel);
            }
        }

        public ActionResult Calendar()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult RedirectToCalendar(string date)
        {            
            return Json(Url.Action("Calendar"));
        }
        
        public ActionResult Report()
        {
            return View();
        }

        public JsonResult GetTasks(double? start, double? end)
        {
            var fromDate = start.ConvertFromUnixTimestamp();
            var toDate = end.ConvertFromUnixTimestamp();

            // var rep = Resolver.Resolve<IEventRepository>();
            // var events = rep.ListEventsForUser(userName, fromDate, toDate);

            var userId = User.Identity.GetUserId();                      
                        
            var rows = taskService.GetTasksForUser(userId, fromDate, toDate).ToList().Select(e =>
                new
                {
                    id = e.EventId,
                    title = e.Event.Title,
                    // url = "http://google.com/",
                    start = e.Event.Start.ToString("s"),
                    end = e.Event.End.ToString("s"),
                    allDay = false
                });

            // var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Edit the task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTask(Guid id)
        {   
            var evt = taskService.GetTask(id.ToString());
                     
            var availableSurveys = persistenceService.GetSurveys();

            var model = new TaskViewModel(evt.Event)
            {
                Survey = surveyService.GetSurvey(evt.Event.SurveyId),
                SurveyId = evt.Event.SurveyId,
                AvailableSurveys = availableSurveys.Select(m => new SurveyEntry {Id = m.Guid.ToString(), Title = m.Title}),
                AvailableUsers = surveillanceService.GetUsers(),
                Users = taskService.GetUsersForTask(evt.EventId)
            };
            
            return View(model);
        }

        /// <summary>
        /// TODO: Consider Update vs Edit 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTask(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {   
                var evt = new Event
                    {
                        Id = model.Id,
                        Title = model.Title,
                        Start = model.Start,
                        End = model.End,
                        //UserId = model.UserId, // Owner
                        SurveyId = model.SurveyId
                    };

                taskService.UpdateTask(evt);                

                if (model.SelectedUsers != null)
                {
                    // Update users list in the repository
                    taskService.UpdateUsersForTask(model.Id, surveillanceService.GetUsers().Select(m => m.Id), model.SelectedUsers);

                    // Notify newly-assigned users on their assignments
                    var availableUsers = taskService.GetUsersForTask(model.Id);
                    foreach (var item in model.SelectedUsers)
                    {
                        var user = availableUsers.FirstOrDefault(m => m.Id == item);
                        if (user != default(AspNetUser))
                        {
                            SendAssignedTaskEmail(user);
                        }
                    }
                }
                
                return RedirectToAction("Calendar");            
            }

            return View(model);
        }

        /// <summary>
        /// Creates the new task.
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTask()
        {   
            var availableSurveys = persistenceService.GetSurveys();

            var model = new TaskViewModel
                {
                    AvailableSurveys = availableSurveys.Select(m => new SurveyEntry{Id = m.Guid.ToString(), Title = m.Title}),
                    AvailableUsers = surveillanceService.GetUsers(),
                    Users = new List<AspNetUser>()
                };

            model.Start = model.End = DateTime.Now;
            return View(model);
        }

        /// <summary>
        /// Creates the new task.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTask(TaskViewModel model)
        {
            if (ModelState.IsValid)
            {
                var evt = new Event
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = model.Title,
                        Start = model.Start,
                        End = model.End,
                        // UserId = model.UserId, // Owner
                        SurveyId = model.SurveyId
                    };

                var @event = taskService.CreateTask(evt);

                taskService.UpdateUsersForTask(evt.Id, new List<string>(), model.SelectedUsers);

                foreach (var user in taskService.GetUsersForTask(evt.Id))
                {
                    // var user = _dbContext.AspNetUsers.FirstOrDefault(m => m.Id == selectedUser);
                    
                    // _dbContext.EventUsers.Add(new EventUser { Event = @event, User = user});

                    SendAssignedTaskEmail(user);
                }

                return RedirectToAction("Calendar");

                /*try
                {
                    //_dbContext.SaveChanges();                    
                }
                catch (DbEntityValidationException e)
                {
                    this.AddErrors(e);
                }*/
            }
            
            return View(model);
        }



        /// <summary>
        /// Sends an email to assignee to notify that the task was assigned.
        /// </summary>
        /// <param name="user"></param>
        private void SendAssignedTaskEmail(AspNetUser user)
        {
            var email = user.Person.Email;
            var baseUrl = EmailHelper.GetDomainNameFromHost(Request.Url.Host);
            var mailService = ServiceManager.GetService<MailService>();
            var template = AccountController.GetEmailTemplate(AccountController.EmailTemplate.EventAssigned);
            
            mailService.SendEmail("donotreply@" + baseUrl, email,
                                  "A task was assigned to you at " + baseUrl, template.ToString(), true, baseUrl);
            
        }
        public ActionResult GreetingPartial()
        {
            var userId = User.Identity.GetUserId();
                        

            var user = surveillanceService.GetUser(userId);

            var model = new PersonViewModel(user.Person) { UserId = user.Id };

            return PartialView("_GreetingPartial", model);
        }

        public ActionResult Export()
        {
            return View();
        }

        public ActionResult Archive()
        {
            return View();
        }
    }
}