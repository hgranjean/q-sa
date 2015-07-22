using Atum.Domain.Common;
using Atum.Domain.Healthcare;
using Atum.Domain.QualityManagement.Auditing;
using Atum.Domain.QualityManagement.Healthcare.Performance;
using Atum.Domain.Security.Domain;
using Atum.Domain.SurveyManagement;
using Atum.Utility;
using Atum.Utility.Diagnostics;
using Microsoft.AspNet.Identity;
using SurveyWeb.Models;
using SurveyWeb.Models.StandardMaintenance;
using SurveyWeb.RuleApp;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class SurveillanceController : Controller
    {
        private readonly SurveillanceManagementServices _surveillanceService;
        private readonly SurveyManagementServices _surveyService;
        private readonly TaskService _taskService;
        private readonly MailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPersistenceServices _persistenceService;
        private readonly LearningServices _learningService;
        private readonly AccountService _accountService;

        public SurveillanceController(SurveillanceManagementServices surveillanceService,
            TaskService taskService,
            SurveyManagementServices surveyService,
            MailService mailService,
            LearningServices learningService,
            IPersistenceServices persistenceService,
            AccountService accountService,
            UserManager<ApplicationUser> userManager)
        {

            _surveillanceService = surveillanceService;
            _surveyService = surveyService;
            _taskService = taskService;
            _userManager = userManager;
            _learningService = learningService;
            _persistenceService = persistenceService;
            _mailService = mailService;
            _accountService = accountService;
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
            Standard model = null;
            if (!string.IsNullOrWhiteSpace(observation))
            {
                model = _learningService.Classify(observation);
            }
            else
            {
                //model = new Standard { Observation = observation };
            }

            var viewModel = new StandardElementViewModel(model);

            return View(model);
        }


        public ActionResult FollowUps(string OwnerId)
        {
            FollowUpsViewModel model = new FollowUpsViewModel();
            model.Add(LoadFollowUp(1));
            model.Add(LoadFollowUp(2));

            return View(model);
        }

        public ActionResult FollowUp(int? followUpId)
        {

            FollowUpViewModel model = LoadFollowUp(followUpId ?? 1);

            return View(model);
        }

        private FollowUpViewModel LoadFollowUp(int followUpId)
        {
            FollowUpViewModel retVal = new FollowUpViewModel();
            retVal.FollowUpId = followUpId;
            retVal.TimeSent = 1;
            retVal.LastSent = DateTime.Parse("04/25/2012");
            retVal.SurveillanceId = "March 2014";
            retVal.InspectionDate = DateTime.Parse("03/20/2012");
            retVal.InspectedBy = "Michelle Kadoun";
            retVal.Category = "Patient Safety";
            retVal.ItemInspected = "Clutter ((0735)";
            retVal.Area = new Area("2 North (027)", 27);
            retVal.ResponsibleParty = new Person("Vicki", "", "Munson");
            retVal.Score = ResponseChoice.NonCompliantString;
            retVal.EstimatedCompletionDate = DateTime.Today.AddDays(2.0D);
            retVal.ItemDetails = "Issue Details_" + followUpId;
            retVal.History = new List<Event>();

            return retVal;
        }


        //
        // GET: /Surveillance/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SurveySchedules()
        {
            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = _userManager.IsInRole(User.Identity.GetUserId(), "Manager");
            ViewBag.ShowTeamMemberContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            var model = GetSurveySchedules(false);

            return View(model);
        }

        private SurveysViewModel GetSurveySchedules(bool isPastDue)
        {
            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = _userManager.IsInRole(User.Identity.GetUserId(), "Manager");
            ViewBag.ShowTeamMemberContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            var surveys = _persistenceService.GetSurveys();

            var userId = User.Identity.GetUserId();

            //Use to be <EventUser>
            IEnumerable<object> events = null;
            if (ViewBag.ShowManagerContent)
            {
                events = _taskService.GetFacilityTasksForUser(userId);
            }
            else
            {
                if (isPastDue)
                {
                    events = _taskService.GetPastDueTasks(userId);
                }
                else
                {
                    //events = _taskService.GetNextTasks(userId);
                }
            }

            var model = new SurveysViewModel();// new SurveysViewModel { SurveysByDate = new Dictionary<int, List<Tuple<EventUser, Template>>>() };

            foreach (var @event in events)
            {
                //var eventSurveys = model.GetOrAddSurveysByDate(@event.Event.Start.ToGroupIndex());

                //var survey = surveys.FirstOrDefault(s => s.Guid.ToString() == @event.Event.SurveyId);

                //if (survey != default(Template))
                //{
                //    var tuple = new Tuple<EventUser, Template>(@event, survey);                    
                //    eventSurveys.Add(tuple);
                //}
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
            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = _userManager.IsInRole(User.Identity.GetUserId(), "Manager");
            ViewBag.ShowTeamMemberContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            var model = GetSurveySchedules(true);

            var nextTasks = GetSurveySchedules(false);

            foreach (var item in nextTasks.SurveysByDate.Keys)
            {
                model.GetOrAddSurveysByDate(item).AddRange(nextTasks.SurveysByDate[item]);
            }

            return View(model);
        }

        public ActionResult PastDueSurveysPartial()
        {
            ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = _userManager.IsInRole(User.Identity.GetUserId(), "Manager");
            ViewBag.ShowTeamMemberContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            var model = new SurveysViewModel();
            try
            {
                model = GetSurveySchedules(true);

                // if (!ViewBag.ShowTeamMemberContent)
                //{
                var nextTasks = GetSurveySchedules(false);

                foreach (var item in nextTasks.SurveysByDate.Keys)
                {
                    model.GetOrAddSurveysByDate(item).AddRange(nextTasks.SurveysByDate[item]);
                }
                //}
            }
            catch (Exception)
            {

                //throw;
            }

            return PartialView("PastDueSurveys", model);
        }


        public ActionResult EditNotes(int surveyId, int questionId)
        {
            return View();
        }
        public ActionResult LoadAttachment(int surveyId, int questionId)
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

            model.SurveyDate = DateTime.Now;

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
            var surveys = _persistenceService.GetSurveys();

            // Filtering out responses by user
            var userId = User.Identity.GetUserId();

            var responses = _surveillanceService.GetResponses(userId);

            var models = new List<TracerViewModel>();
            foreach (var response in responses)
            {
                try
                {
                    var tracerModel = _persistenceService.LoadTracer(response);
                    LoadTracerReferenceData(tracerModel);

                    // TODO: remove these assignments
                    tracerModel.ResponseId = response;
                    tracerModel.SurveyTitle = surveys.FirstOrDefault(m => m.Id == tracerModel.SurveyId).Title;
                    models.Add(tracerModel);
                }
                catch (FileNotFoundException ex)
                {
                    DebugUtil.WriteLine("Response file was not found: {0}. {1}", response, ex.ToString());
                }
            }

            return new CompletedSurveyViewModel(models);
        }

        public ActionResult CompletedObservationsPartial()
        {
            var model = GetCompletedObservations();

            return PartialView("CompletedObservations", model);
        }

        private CompletedObservationsViewModel GetCompletedObservations()
        {
            var surveys = _persistenceService.GetSurveys();
            Contract.Assert(surveys != null);

            // Filtering out responses by user
            var userId = User.Identity.GetUserId();

            var responseIds = _surveillanceService.GetResponses(userId);

            var models = new List<ObservationViewModel>();
            foreach (var responseId in responseIds)
            {
                try
                {
                    var tracerModel = _persistenceService.LoadTracer(responseId);
                    LoadTracerReferenceData(tracerModel);

                    // TODO: remove these assignments
                    var survey = surveys.FirstOrDefault(m => m.Id == tracerModel.SurveyId);
                    tracerModel.ResponseId = responseId;
                    tracerModel.SurveyTitle = survey != null ? survey.Title : String.Empty;

                    foreach (var question in tracerModel.Responses.Where(m => m != null))
                    {
                        //var observation = new Observation(tracerModel.Surveyor, question.Response.Question,
                        //    question.Response.Answer);

                        //models.Add(new ObservationViewModel(observation) { ResponseId = responseId });
                    }
                }
                catch (NullReferenceException ex)
                {
                    DebugUtil.WriteLine("Problem while parsing a response {0}. {1}", responseId, ex.ToString());
                }
                catch (FileNotFoundException ex)
                {
                    DebugUtil.WriteLine("Response file was not found: {0}. {1}", responseId, ex.ToString());
                }
            }

            return new CompletedObservationsViewModel(models);
        }

        public ActionResult ViewCompletedSurvey(string id)
        {
            var availableResponses = _persistenceService.GetResponses();

            foreach (var response in availableResponses)
            {
                if (response.Contains(id))
                {
                    var model = _persistenceService.LoadTracer(id);

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
            Survey survey = null; // = LoadSurvey("Template Template 1");
            if (surveyId.HasValue)
            {
                survey = _persistenceService.GetSurvey(surveyId.Value);

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
            retVal.Areas = LoadAreas(retVal.FacilityId.ToString());
            retVal.Surveyors = LoadSurveyors();
            retVal.Departments = LoadDepartments();
            retVal.FloorNumber = 3;
        }

        private int _questionChoiceNextId = 0;

        private void SetQuestionChoices(Question question)
        {
            //question.AddChoice(ResponseChoice.CompliantString).SetIdInternal(_questionChoiceNextId++);
            question.AddChoice(ResponseChoice.NonCompliantString).SetIdInternal(_questionChoiceNextId++);
            question.AddChoice(ResponseChoice.NAString).SetIdInternal(_questionChoiceNextId++);
            // question.AddChoice(NotScoredString); // AS - Not Valid choice
            question.AddChoice(ResponseChoice.FollowUpCompletedString).SetIdInternal(_questionChoiceNextId++);
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

        //private TableOfContents LoadTableOfContents()
        //{
        //    var toc = new TableOfContents();
        //    toc.AddElement("Element Title", "Element Title");
        //    return toc;
        //}

        private IEnumerable<Department> LoadDepartments()
        {
            return _surveillanceService.GetDepartments();
        }

        private IEnumerable<Person> LoadSurveyors()
        {
            return _surveillanceService.GetPersons();
        }

        private IEnumerable<Area> LoadAreas(string hospitalId)
        {
            return _surveillanceService.GetAreas(hospitalId);
        }

        private IEnumerable<Facility> LoadFacilities()
        {
            return _surveillanceService.GetFacilities();
        }

        private IEnumerable<Building> LoadBuildings()
        {
            return _surveillanceService.GetBuildings();
        }

        public ActionResult ViewReference(string standardId)
        {
            DocumentElement model = new Chapter("", "");
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
                    var userId = User.Identity.GetUserId();


                    if (_userManager.IsInRole(User.Identity.GetUserId(), "Administrator"))
                    {
                        return RedirectToAction("Dashboard", "QTrackerAdmin");

                    }
                    else if (_userManager.IsInRole(User.Identity.GetUserId(), "Manager"))
                    {
                        return RedirectToAction("Dashboard", "QualityManager");

                    }
                    else if (_userManager.IsInRole(User.Identity.GetUserId(), "Team Member"))
                    {
                        return RedirectToAction("Dashboard", "QualityAuditor");

                    }

                    ViewBag.ShowAdminContent = _userManager.IsInRole(userId, "Administrator");
                    ViewBag.ShowManagerContent = _userManager.IsInRole(userId, "Manager");
                    ViewBag.ShowTeamMemberContent = _userManager.IsInRole(userId, "Team Member");

                }
                catch (Exception ex)
                {
                    DebugUtil.WriteLine(ex.ToString());

                    Session.Abandon();

                    RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        public ActionResult NewDashboard()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var userId = User.Identity.GetUserId();
                    ViewBag.ShowAdminContent = _userManager.IsInRole(userId, "Administrator");
                    ViewBag.ShowManagerContent = _userManager.IsInRole(userId, "Manager");
                    ViewBag.ShowTeamMemberContent = _userManager.IsInRole(userId, "Team Member");
                }
                catch (Exception ex)
                {
                    DebugUtil.WriteLine(ex.ToString());

                    Session.Abandon();

                    RedirectToAction("Index", "Home");
                }
            }

            return View();
        }


        [HttpPost]
        public ActionResult SaveSurvey(TracerViewModel viewModel, FormCollection values)
        {
            var questions = new Questions();
            var responses = new Responses();

            int qIndex = 0;
            bool isObservation = false;
            var responsesViewModels = new ResponseViewModel[100];

            var survey = _persistenceService.GetSurvey(viewModel.SurveyId);

            foreach (var questionGroup in survey.QuestionGroups)
            {
                foreach (var question in questionGroup.Value.Questions)
                {
                    questions.Add(question);
                    var value = values.GetValue("Responses[" + qIndex  /*question.Number qIndex  */ + "]");
                    if (value != null)
                    {
                        var responseId = (int)value.ConvertTo(typeof(int));

                        var choice = question.ResponseChoices.FirstOrDefault(r => r.Id == responseId);
                        if (choice != default(ResponseChoice))
                        {
                            var response = new Response(question, choice);
                            responses.Add(response);

                            responsesViewModels[qIndex] = new ResponseViewModel(response) { ResponseId = responseId };
                        }
                    }
                    qIndex++;
                }

                // For new observations, add them to the appropriate group
                var observationText = values["txtObservation" + questionGroup.Key];
                if (!String.IsNullOrWhiteSpace(observationText))
                {
                    // Add observation
                    var newQuestion = questionGroup.Value.AddQuestion(observationText, QuestionType.SelectOne);
                    var classifyModel = _learningService.Classify(observationText);
                    //newQuestion.TOCReference = classifyModel.StandardId;
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

            // Create new response unless already created

            if (String.IsNullOrEmpty(viewModel.ResponseId))
            {
                var userId = User.Identity.GetUserId();

                var responseEntry = _surveillanceService.AddResponse(userId);

                viewModel.ResponseId = responseEntry.Id;
            }

            _persistenceService.SaveTracer(viewModel);

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

            var rows = _taskService.GetTasksForUser(userId, fromDate, toDate).ToList().Select(e =>
                new
                {
                    //id = e.EventId,
                    //title = e.Event.Title,
                    // url = "http://google.com/",
                    //start = e.Event.Start.ToString("s"),
                    //end = e.Event.End.ToString("s"),
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
            var evt = _taskService.GetTask(id.ToString());

            var availableSurveys = _persistenceService.GetSurveys();

            var model = new TaskViewModel(evt)
            {
                //Template = _surveyService.GetSurveyEntry(evt.Event.SurveyId),
                ////SurveyId = evt.Event.SurveyId,
                //AvailableSurveys = availableSurveys.Select(m => new SurveyEntry {Id = m.Guid.ToString(), Title = m.Title}),
                //AvailableUsers = _accountService.GetUsers(),
                //Users = _taskService.GetUsersForTask(evt.Id)
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
                        //SurveyId = model.SurveyId
                    };

                _taskService.UpdateTask(evt);

                if (model.SelectedUsers != null)
                {
                    // Update users list in the repository
                    _taskService.UpdateUsersForTask(model.Id, _accountService.GetUsers().Select(m => m.Id), model.SelectedUsers);

                    // Notify newly-assigned users on their assignments
                    var availableUsers = _accountService.GetUsers();
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
            var availableSurveys = _persistenceService.GetSurveys();

            var model = new TaskViewModel
                {
                    AvailableSurveys = availableSurveys.Select(m => new SurveyEntry { Id = m.Guid.ToString(), Title = m.Title }),
                    AvailableUsers = _accountService.GetUsers(),
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
                        //SurveyId = model.SurveyId
                    };

                var @event = _taskService.CreateTask(evt);

                _taskService.UpdateUsersForTask(evt.Id, new List<string>(), model.SelectedUsers);

                foreach (var user in _taskService.GetUsersForTask(evt.Id))
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
            // var mailService = ServiceManager.GetService<MailService>();
            var template = _mailService.GetEmailTemplate(EmailTemplate.EventAssigned);

            _mailService.SendEmail("donotreply@" + baseUrl, email,
                                  "A task was assigned to you at " + baseUrl, template.ToString(), true, baseUrl);

        }
        public ActionResult GreetingPartial()
        {
            var userId = User.Identity.GetUserId();

            var user = _accountService.GetUser(userId);

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

        [HttpPost]
        public ActionResult Delete(string responseId)
        {
            var userId = User.Identity.GetUserId();

            _surveillanceService.DeleteResponse(responseId);

            _persistenceService.DeleteResponse(responseId);

            return RedirectToAction("Dashboard");
        }

        public ActionResult TakePhoto(string surveyId, string questionId)
        {
            var viewModel = new PhotoViewModel { SurveyId = surveyId, QuestionId = questionId };

            return View(viewModel);
        }
        public ActionResult TakePhoto(string responseId)
        {
            var viewModel = new PhotoViewModel { ResponseId = responseId };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult TakePhoto(PhotoViewModel model)
        {
            string fileName = Guid.NewGuid().ToString("d") + ".png";
            string fileNameWitPath = Path.Combine(Server.MapPath("~/Store/Photos"), fileName);

            using (var fs = new FileStream(fileNameWitPath, FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    byte[] data = Convert.FromBase64String(model.ImageData);
                    bw.Write(data);
                    bw.Close();
                }
                fs.Close();
            }

            model.FileName = fileName;
            model.IsPublished = true;
            model.UserName = User.Identity.Name;
            model.CreatedDateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                // db.DrawingModels.Add(model);
                // db.SaveChanges();

                // return RedirectToAction("Index");
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        public ActionResult AssignTo(string id)
        {
            var surveyors = LoadSurveyors();

            var currentUserId = _persistenceService.LoadTracer(id).SurveyorId;

            var viewModel = new AssignToViewModel { ResponseId = id, Surveyors = surveyors, AssignedTo = currentUserId.ToString() };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignTo(AssignToViewModel viewModel)
        {
            var tracer = _persistenceService.LoadTracer(viewModel.ResponseId);
            tracer.SurveyorId = new Guid(viewModel.AssignedTo);
            _persistenceService.SaveTracer(tracer);
            return RedirectToAction("Dashboard");
        }


        public ActionResult AddObservation()
        {
            // Initialize a new tracerviewmodel container, add reference data etc.
            var viewModel = new TracerViewModel();

            viewModel.ResponseId = Guid.NewGuid().ToString();

            var userId = User.Identity.GetUserId();

            var personId = _accountService.GetUser(userId).PersonId;

            viewModel.SurveyorId = new Guid(personId);

            viewModel.SurveyDate = DateTime.Now;

            LoadTracerReferenceData(viewModel);

            //            ViewBag.StandardElement = new StandardElementViewModel(new Standard { Observation = string.Empty});

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddObservation(TracerViewModel viewModel, StandardElementViewModel standardElementViewModel, FormCollection values)
        {
            // Validate partials TODO: Split EPIds and make validdation against standardElementViewModel
            if (String.IsNullOrWhiteSpace(values["EPIds"]))
            {
                ModelState.AddModelError("EPIds", "EP is required");
            }

            if (ModelState.IsValid)
            {
                var observationText = values["ObservationText"];

                if (!String.IsNullOrWhiteSpace(observationText))
                {
                    // Add observation

                    var newQuestion = viewModel
                        .AddDefaultQuestionGroup()
                        .AddQuestion(observationText, QuestionType.SelectOne);

                    var classifyModel = _learningService.Classify(observationText);
                    //newQuestion.TOCReference = classifyModel.StandardId;

                    viewModel.AddResponse(newQuestion);

                    var userId = User.Identity.GetUserId();

                    var responseEntry = _surveillanceService.AddResponse(userId);

                    viewModel.ResponseId = responseEntry.Id;
                }

                _persistenceService.SaveTracer(viewModel);

                return RedirectToAction("Dashboard");
            }

            ViewBag.StandardElement = standardElementViewModel;

            LoadTracerReferenceData(viewModel);

            return View(viewModel);
        }

        public PartialViewResult EditNote(int surveyId, int questionGroupId, int questionId, Guid? responseId)
        {
            var viewModel = new EditNoteViewModel
            {
                NoteId = Guid.NewGuid().ToString(), // Note that MVC cannot roundtrip Guid types
                SurveyId = surveyId,
                QuestionGroupNumber = questionGroupId,
                QuestionId = questionId,
                ResponseId = responseId.GetValueOrDefault().ToString()
            };

            return PartialView("_EditNotePartial", viewModel);
        }

        [HttpPost]
        public JsonResult EditNote(EditNoteViewModel viewModel, FormCollection values)
        {
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrWhiteSpace(viewModel.NoteText))
                {
                    // Save response

                    if (!String.IsNullOrWhiteSpace(viewModel.ResponseId))
                    {
                        var userId = User.Identity.GetUserId();

                        var responseEntry = _surveillanceService.AddResponse(userId);

                        viewModel.ResponseId = responseEntry.Id;
                    }
                    else
                    {
                        throw new InvalidOperationException("Unable to save note without response file.");
                        // TODO: Save note in the existing response file.   
                    }

                    _persistenceService.SaveNote(viewModel);
                }

                // return RedirectToAction("Dashboard");
            }

            return new JsonResult { Data = "success" };
        }
    }
}