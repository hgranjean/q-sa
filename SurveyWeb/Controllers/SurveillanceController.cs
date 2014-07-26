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

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class SurveillanceController : Controller
    {
        private AtumSurveillanceContext _dbContext = null;
        
        public SurveillanceController()
           : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            _dbContext = new AtumSurveillanceContext();
        }

        public SurveillanceController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
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
            return View();
        }




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
            
            var model = new SurveysViewModel { Surveys = new Surveys() };
            model.Surveys.AddRange(surveys);

            ViewBag.ShowAdminContent = UserManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowSurveyorContent = UserManager.IsInRole(User.Identity.GetUserId(), "Surveyor");
            
            return View(model);
        }

        public ActionResult SurveySchedules()
        {
            var surveys = SurveyViewModel.GetSurveys();

            var userId = User.Identity.GetUserId();
            var events = _dbContext.EventUsers.Include(m => m.Event.Survey).Where(m => m.UserId == userId);

            var model = new SurveysViewModel { SurveysByDate = new Dictionary<int, Surveys>()};
            
            foreach (var @event in events)
            {   
                var eventSurveys = model.GetOrAddSurveysByDate(@event.Event.Start.ToGroupIndex());

                var survey = surveys.FirstOrDefault(s => s.Guid.ToString() == @event.Event.SurveyId);
                
                eventSurveys.Add(survey);    
            }

            return View(model);
        }

        public ActionResult PastDueSurveys()
        {
            var surveys = SurveyViewModel.GetSurveys();

            var userId = User.Identity.GetUserId();
            var events = _dbContext.EventUsers.Include(m => m.Event.Survey).Where(m => m.UserId == userId && m.Event.Start <= DateTime.Now);

            var model = new SurveysViewModel { SurveysByDate = new Dictionary<int, Surveys>() };

            foreach (var @event in events)
            {
                var eventSurveys = model.GetOrAddSurveysByDate(@event.Event.Start.ToGroupIndex());

                var survey = surveys.FirstOrDefault(s => s.Guid.ToString() == @event.Event.SurveyId);

                eventSurveys.Add(survey);
            }

            return View(model);
        }

        public ActionResult DeleteSurvey(long id)
        {
            var model = SurveyViewModel.GetSurveys().FirstOrDefault(m => m.ID == id);

            return View(model);
        }
        
        [HttpPost]
        public ActionResult DeleteSurvey(SurveyViewModel model)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            persistenceService.DeleteSurvey(model.Survey.ID.ToString());

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
                //survey = LoadSurvey("Survey Title 1");
            }
            
            var model = new SurveyViewModel(survey);

            return View(model);            
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

            if (model.SurveyTypeId == (int)SurveyType.Audit)
            {
                return RedirectToAction("SurveyDesign", new {id = id.ToString()});
            }
            
            return View(model);
        }

        public ActionResult CompletedSurveys()
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var surveys = persistenceService.GetSurveys();
            
            // Filtering out responses by user
            var userId = User.Identity.GetUserId();
            var responses = _dbContext.Responses.Where(m => m.UserId == userId).Select(m => m.Id);

            var models = new List<TracerViewModel>();
            foreach (var response in responses)
            {
                var tracerModel = persistenceService.LoadTracer(response);
                tracerModel.SurveyTitle = surveys.FirstOrDefault(m => m.ID == tracerModel.SurveyId).Title;
                models.Add(tracerModel);
            }

            var model = new CompletedSurveyViewModel(models);

            return View(model);
        }

        public ActionResult ViewCompletedSurvey(string responseId)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var availableResponses = persistenceService.GetResponses();

            foreach (var response in availableResponses)
            {
                if (response.Contains(responseId))
                {   
                    var model = persistenceService.LoadTracer(responseId);

                    LoadTracerReferenceData(model);

                    LoadSurveyData(model);

                    ViewBag.IsReadOnly = true;

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
            Survey survey = null; // = LoadSurvey("Survey Template 1");
            if (surveyId.HasValue)
            {
                var persistenceService = ServiceManager.GetService<PersistenceServices>();
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

        private void LoadTracerReferenceData(TracerViewModel retVal)
        {
            retVal.Buildings = LoadBuildings();
            retVal.Facilities = LoadFacilities();
            retVal.Areas = LoadAreas();
            retVal.Surveyors = LoadSurveyors();
            retVal.Departments = LoadDepartments();
            retVal.FloorNumber = 3;
        }

        //private Survey LoadSurvey(string title)
        //{
        //    var survey = new Survey(title);

        //    ////Set Survey Type - Overwrite Survey Type
        //    survey.SurveyType = SurveyType.Audit;

        //    // Step 2 - Initialize TOC

        //    //Survey Basis Document (assert that we can see the TOCElements
        //    var surveyBasis = new SurveyBasis();
        //    surveyBasis.TableOfContents = LoadTableOContents();

        //    Question question = null;
        //    var qGroup0211 = survey.AddQuestionGroup("0211_Doors ");
        //    question = qGroup0211.AddQuestion("0211", "No items covering doors, i.e. decorations, paper, etc. ", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0214 = survey.AddQuestionGroup("0214_Adequate Lighting");
        //    question = qGroup0214.AddQuestion("0214", "Lighting is adequate. ", QuestionType.SelectOne);
        //    question.BasisReference = new TOCElement("Std: LS.02.01.20 EP27 ");
        //    SetQuestionChoices(question);

        //    var qGroup0215 = survey.AddQuestionGroup("0215_ Personal Items");
        //    question = qGroup0215.AddQuestion("0215", "No items stored under the sink in kitchen area.  ", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0218 = survey.AddQuestionGroup("0218_Unoccupied Rooms ");
        //    question = qGroup0218.AddQuestion("0218", "Unoccupied rooms are locked.", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0219 = survey.AddQuestionGroup("0219_ Violent/Disruptive Behavior");
        //    question = qGroup0219.AddQuestion("0219", "How do you respond to violent or disruptive behavior?", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0220 = survey.AddQuestionGroup("0220_Weapons");
        //    question = qGroup0220.AddQuestion("0220", "How do you respond to violent or disruptive behavior with weapons? ", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0221 = survey.AddQuestionGroup("0221_Authorized Identification");
        //    question = qGroup0221.AddQuestion("0221", "Are all individuals in area wearing their authorized identification according to hospital policy?", QuestionType.SelectOne);
        //    SetQuestionChoices(question);


        //    var qGroup0222 = survey.AddQuestionGroup("0222_Emergency Numbers Posted");
        //    question = qGroup0222.AddQuestion("0222", "Emergency numbers are visibly posted. ", QuestionType.SelectOne);
        //    question.BasisReference = new TOCElement("Std: EC.02.01.01 EP10 ");
        //    SetQuestionChoices(question);

        //    var qGroup0223 = survey.AddQuestionGroup("0223_Gas Cylinders Secured");
        //    question = qGroup0223.AddQuestion("0223", "Are gas cylinders properly secured? ", QuestionType.SelectOne);
        //    question.BasisReference = new TOCElement("Std: EC.02.03.01 EP1");
        //    SetQuestionChoices(question);

        //    return survey;
        //}

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
            // yield return new Person { FirstName = "Joe", MiddleName = "D", LastName = "Surveyor" };
            // yield return new Person { FirstName = "Henry", MiddleName = "M", LastName = "TracerDude" };
            return _dbContext.Persons;
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
            ViewBag.ShowAdminContent = UserManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            ViewBag.ShowManagerContent = UserManager.IsInRole(User.Identity.GetUserId(), "Manager");
            ViewBag.ShowTeamMemberContent = UserManager.IsInRole(User.Identity.GetUserId(), "Team Member");
            
            return View();
        }

        [HttpPost]
        public ActionResult SaveSurveillance(TracerViewModel viewModel, FormCollection values)
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var survey = persistenceService.GetSurvey(viewModel.SurveyId);
            
            var questions = new Questions();
            var responses = new Responses();

            int qIndex = 0;
            var responsesViewModels = new ResponseViewModel[100];
            foreach (var questionGroup in survey.QuestionGroups)
            {
                foreach (var question in questionGroup.Value.Questions)
                {
                    questions.Add(question);
                    var value = values.GetValue("Responses[" + question.Number /* qIndex  */ + "]");
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

            var userId = User.Identity.GetUserId();

            var user = _dbContext.AspNetUsers.FirstOrDefault(m => m.Id == userId);
            
            var responseEntry = new ResponseEntry {Id = Guid.NewGuid().ToString("d"), User = user};

            _dbContext.Responses.Add(responseEntry);

            _dbContext.SaveChanges();

            viewModel.ResponseId = responseEntry.Id;

            persistenceService.SaveTracer(viewModel);
            
            return View("SurveyAnalysis", analysisViewModel);
        }



        public ActionResult Calendar()
        {
            return View();
        }
        
        public ActionResult Report()
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

            var rows = _dbContext.Events.ToList().Select(e =>
                new
                {
                    id = e.Id,
                    title = e.Title,
                    start = e.Start.ToString("s"),
                    end = e.End.ToString("s"),
                    allDay = false
                });

            // var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// TODO: Add descriptive comment here.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditEvent(Guid id)
        {
            var evt = _dbContext.Events.FirstOrDefault(m => m.Id == id.ToString());

            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var availableSurveys = persistenceService.GetSurveys();

            var model = new EventViewModel(evt)
            {
                Survey = _dbContext.Surveys.FirstOrDefault(m => m.Id == evt.SurveyId),
                SurveyId = evt.SurveyId,
                AvailableSurveys = availableSurveys.Select(m => new SurveyEntry {Id = m.Guid.ToString(), Title = m.Title}),
                AvailableUsers = _dbContext.AspNetUsers,
                Users = from a in _dbContext.EventUsers
                        join b in _dbContext.AspNetUsers on a.UserId equals b.Id
                        where a.EventId == evt.Id
                        select b
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
        public ActionResult EditEvent(EventViewModel model)
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

                _dbContext.Events.Attach(evt);
                _dbContext.Entry(evt).CurrentValues.SetValues(evt);
                _dbContext.Entry(evt).State = EntityState.Modified;

                if (model.SelectedUsers != null)
                {
                    // Remove unselected items

                    var availableUsers = _dbContext.AspNetUsers;
                    foreach (var item in availableUsers)
                        {
                            if (model.SelectedUsers.Count(userId => userId == item.Id) == 0)
                            {
                                var toDelete = _dbContext.EventUsers.First(eventUser => eventUser.EventId == model.Id && eventUser.UserId == item.Id);

                                _dbContext.EventUsers.Remove(toDelete);
                            }
                        }
                    

                    // Add newly selected items
                    foreach (var userId in model.SelectedUsers)
                    {
                        if (model.Users.Count(user => user.Id == userId) == 0)
                        {
                            _dbContext.EventUsers.Add(new EventUser { EventId = model.Id, UserId = userId });
                        }
                    }

                    _dbContext.SaveChanges();
                }

                // Update users on their assignments
                if (model.SelectedUsers != null)
                {
                    
                    var availableUsers = _dbContext.AspNetUsers;
                    foreach (var item in model.SelectedUsers)
                    {
                        
                        var user = availableUsers.FirstOrDefault(m => m.Id == item);
                        if (user != default(AspNetUser))
                        {
                            SendAssignedEventEmail(user);
                        }
                    }
                }
            
                try
                {
                    _dbContext.SaveChanges();

                    return RedirectToAction("Calendar");
                }
                catch (DbEntityValidationException e)
                {
                    this.AddErrors(e);
                }
            }

            return View(model);
        }

        /// <summary>
        /// TODO: Specify Type of Event i.e. Change Action Method Name to be more descriptive
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateEvent()
        {
            var persistenceService = ServiceManager.GetService<PersistenceServices>();
            var availableSurveys = persistenceService.GetSurveys();

            var model = new EventViewModel
                {
                    AvailableSurveys = availableSurveys.Select(m => new SurveyEntry{Id = m.Guid.ToString(), Title = m.Title}), // _dbContext.Surveys,
                    AvailableUsers = _dbContext.AspNetUsers,
                    Users = new List<AspNetUser>()
                };

            model.Start = model.End = DateTime.Now;
            return View(model);
        }

        /// <summary>
        /// TODO: Specify Type of Event i.e. Change Action Method Name to be more descriptive
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEvent(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var @event = _dbContext.Events.Add(new Event
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = model.Title,
                        Start = model.Start,
                        End = model.End,
                        // UserId = model.UserId, // Owner
                        SurveyId = model.SurveyId
                    });

                foreach (var selectedUser in model.SelectedUsers)
                {
                    var user = _dbContext.AspNetUsers.FirstOrDefault(m => m.Id == selectedUser);
                    
                    _dbContext.EventUsers.Add(new EventUser { Event = @event, User = user});

                    SendAssignedEventEmail(user);
                }

                try
                {
                    _dbContext.SaveChanges();

                    return RedirectToAction("Calendar");
                }
                catch (DbEntityValidationException e)
                {
                    this.AddErrors(e);
                }
            }
            
            return View(model);
        }



        /// <summary>
        /// TODO: Specify Type of Event i.e. Change Action Method Name to be more descriptive
        /// </summary>
        /// <param name="user"></param>
        private void SendAssignedEventEmail(AspNetUser user)
        {
            var email = user.Person.Email;
            var baseUrl = Request.Url.Host == "localhost" ? "localhost.com" : Request.Url.Host;
            var mailService = ServiceManager.GetService<MailService>();
            var template = AccountController.GetEmailTemplate(AccountController.EmailTemplate.EventAssigned);
            
            mailService.SendEmail("donotreply@" + baseUrl, email,
                                  "An event was assigned to you at " + baseUrl, template.ToString(), true, baseUrl);
            
        }
    }
}