using Atum.Domain.Common;
using Atum.Utility.XML;
using SurveyWeb.Models;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class StandardController : Controller
    {
        private readonly StandardsManagementServices _standardManagementService;

        public StandardController(StandardsManagementServices standardManagementService)
        {
            _standardManagementService = standardManagementService;
        }


        /// <summary>
        /// Standard Content
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public TOCElement GetViewModel(string Id)
        {

            var model = new TOCElement(Id);

            if (Id == "LS.02.01.20 EP27")
            {
                //model.Content = LoadContent(Id);
            }

            if (Id == "LS.04.03.02")
            {
                string appPath = AppDomain.CurrentDomain.RelativeSearchPath;

                appPath = appPath + @"\\..\RuleApp\";
                //TODO: Get From Standard Services
                model = (TOCElement)XmlSerializationUtility.GetObjectFromFile(appPath + @"Standards\" + Id + ".xml", typeof(TOCElement));
            }

            return model;
        }

        /// <summary>
        /// Standard Content
        /// </summary>
        /// <returns></returns>
        public IEnumerable GetTOCs()
        {
            yield return new KeyValuePair<string, TOCElement>("", TOCElement.None);
            yield return new KeyValuePair<string, TOCElement>("LS.02.01.20 EP27", GetViewModel("LS.02.01.20 EP27"));
            yield return new KeyValuePair<string, TOCElement>("LS.04.03.02", GetViewModel("LS.04.03.02"));
        }

        public ActionResult Document(int? id)
        {
            Models.StandardDocumentViewModel model = new Models.StandardDocumentViewModel();
            if (id.HasValue)
            {
                //Load Document
                model = loadDocument(id);

            }

            //model.FacultyList = new SelectList(EducationServices.GetAllFaculty(), "Id", "Name");

            return View(model);
        }

        private StandardDocumentViewModel loadDocument(int? id)
        {
            var retVal = new Models.StandardDocumentViewModel();
            retVal.Title = "Proposed Core Reqirements - All chapters Hospital Accreditation Program";
            retVal.TableOfContents = LoadTableOfContent();
            return retVal;
        }

        private IEnumerable<TOCElementViewModel> LoadTableOfContent()
        {
            var tocElement = new TOCElementViewModel();
            tocElement.Title = "Environment of Care (EC)";
            tocElement.Key = "EC";
            yield return tocElement;

            tocElement = new TOCElementViewModel();
            tocElement.Title = "Emergency Management (EM) ";
            tocElement.Key = "EM";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Human Resources (HR) ";
            tocElement.Key = "HR";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Infection Prevention and Control (IC) ";
            tocElement.Key = "IC";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Information Management (IM) ";
            tocElement.Key = "IM";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Leadership (LD) ";
            tocElement.Key = "LD";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Life Safety (LS)";
            tocElement.Key = "LS";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Medication Management (MM) ";
            tocElement.Key = "MM";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Provision of Care, Treatment, and Services (PC) ";
            tocElement.Key = "PC";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Performance Improvement (PI) ";
            tocElement.Key = "PC";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Record of Care, Treatment, and Services (RC) ";
            tocElement.Key = "RC";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Rights and Responsibilities of the Individual (RI) ";
            tocElement.Key = "RI";
            yield return tocElement;
            
            tocElement = new TOCElementViewModel();
            tocElement.Title = "Waived Testing (WT) ";
            tocElement.Key = "WT";
            yield return tocElement;            
        }


        public ActionResult Chapter(string chapterId)
        {
            var model = _standardManagementService.GetChapter(chapterId);
            //model.TableOfContents = new List<Models.TOCElementViewModel>();            
            return View(model);
        }


        public ActionResult StandardElement(string standardElementId)
        {
            var model = _standardManagementService.GetStandardElement(standardElementId);            
            model.Key = standardElementId;            
            return View(model);
        }

        public ActionResult PerformanceElement(string standardElementId, string performanceItemId)
        {
            var model = _standardManagementService.GetPerformanceElementViewModel(standardElementId, performanceItemId);            
            model.StandardId = standardElementId;            
            return View(model);
        }
    }
}