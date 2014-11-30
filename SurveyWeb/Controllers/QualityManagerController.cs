using Atum.Domain.SurveyManagement;
using SurveyWeb.Models.QualityManager;
using SurveyWeb.Models.QualityManager.SurveillanceManagement;
using SurveyWeb.Models.QualityManager.SurveillanceTracking;
using SurveyWeb.Models.QualityManager.TemplatesManagement;
using SurveyWeb.Models.StandardMaintenance;
using SurveyWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class QualityManagerController : Controller
    {

        private readonly SurveillanceManagementServices _surveillanceServices;
        private readonly StandardsManagementServices _standardManagementServices;

        public QualityManagerController(SurveillanceManagementServices surveillanceService, StandardsManagementServices standardManagementServices)           
        {
            _surveillanceServices = surveillanceService;
            _standardManagementServices = standardManagementServices;
        }


        // GET: QualityManager
        public ActionResult Dashboard()
        {
            ManagerUserModel model = new ManagerUserModel();
            
            return View(model);
        }

        //Manage Surveillance
        public ActionResult ManageSurveillances()
        {
            return View();
        }
        
        /*Manage Surveillances Subtasks*/
        public ActionResult TrackFollowUps()
        {
            return View();
        }


        /*Surveillance Templates Subtasks*/
        /// <summary>
        /// View List of Surveys
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveillanceTemplates()
        {
            var model = new SurveysViewModel { Surveys = new Surveys() };

            //model.Surveys.AddRange(surveys);

            //ViewBag.ShowAdminContent = _userManager.IsInRole(User.Identity.GetUserId(), "Administrator");
            //ViewBag.ShowSurveyorContent = _userManager.IsInRole(User.Identity.GetUserId(), "Team Member");

            return View(model);
        }

        /*Maintain Performance Guidelines and Standards*/
        /// <summary>
        /// View list of Performance Guidelines and Standards
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageStandards()
        {
            var model = new ManageStandardsViewModel();
            model.Guidelines = _standardManagementServices.GetStandardDocuments();
            //Model needs a list of DocumentViewModel
            return View(model);
        }

        /// <summary>
        /// TODO: Replace with appropriate view model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Standard(int? id)
        {
            var model = new StandardDocumentViewModel();
            if (id.HasValue)
            {
                //Load Document
                model = _standardManagementServices.LoadDocument(id);
            }
            else
            {
                model.TableOfContents = new List<TOCElementViewModel>();
            }

            return View(model);
        }

        /// <summary>
        /// Review The Schedule
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveillanceSchedule()
        {
            var model = new SurveillanceManagementModel();
            model.AreaAnnualSchedule = (new List<SurveillanceScheduleAnnualViewModel>());
            model.AreaAnnualSchedule.AddRange(_surveillanceServices.GetAreaAnnualSchedule());
 
            
            return View(model);
        }

        public ActionResult ScheduleSurveillance()
        {
            int managerId = 1;
            var availableSurveys = this._surveillanceServices.GetSurveysForManager(managerId);

            var model = new SurveilanceViewModel()
            {
                AvailableSurveys = availableSurveys.Select(m => new SurveyEntry { Id = m.Guid.ToString(), Title = m.Title }),
                //AvailableUsers = _accountService.GetUsers(),
                //Users = new List<AspNetUser>()
            };

            model.Start = model.End = DateTime.Now;
            return View(model);
        }

        /*Surveillance Schedule Subtasks*/
        public ActionResult ManageResources()
        {
            return View();
        }

        /// <summary>
        /// Surveillance Completion
        /// </summary>
        /// <returns></returns>
        public ActionResult SurveillanceCompletion()
        {
            var model = new SurveillanceTrackingViewModel();
            model.AreaSurveillanceCompletion = (new List<SurveillanceCompletionViewModel>());
            model.AreaSurveillanceCompletion.AddRange(_surveillanceServices.GetAreaSurveillanceCompletion());

            return View(model);
        }

        public ActionResult ManageAlerts()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult IdentifyTrends()
        {
            return View();
        }


        public ActionResult ManagePIPrograms()
        {
            return View();
        }

    }
}