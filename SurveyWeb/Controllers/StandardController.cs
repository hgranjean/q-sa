using Atum.Domain.Common;
using Atum.Utility.XML;
using SurveyWeb.Models;
using SurveyWeb.Repository;
using SurveyWeb.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class StandardController : Controller
    {
        private readonly ISurveyStore _store;
        private readonly StandardsManagementServices _standardManagementService;

        public StandardController(ISurveyStore store, StandardsManagementServices standardManagementService)
        {
            _store = store;
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
                var appPath = _store.GetPath();
                //TODO: Get From Standard Services
                model = (TOCElement)XmlSerializationUtility.GetObjectFromFile(Path.Combine(appPath, "Standards", Id + ".xml"), typeof(TOCElement));
            }

            return model;
        }

        /// <summary>
        /// Standard Content
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TOCElement>> GetTOCs()
        {
            yield return new KeyValuePair<string, TOCElement>(string.Empty, TOCElement.None);
            yield return new KeyValuePair<string, TOCElement>("LS.02.01.20 EP27", GetViewModel("LS.02.01.20 EP27"));
            yield return new KeyValuePair<string, TOCElement>("LS.04.03.02", GetViewModel("LS.04.03.02"));
        }

        public ActionResult Document(int? id)
        {
            var model = new StandardDocumentViewModel();
            if (id.HasValue)
            {
                //Load Document
                model = LoadDocument(id);
            }

            //model.FacultyList = new SelectList(EducationServices.GetAllFaculty(), "Id", "Name");

            return View(model);
        }

        private StandardDocumentViewModel LoadDocument(int? id)
        {
            var retVal = new StandardDocumentViewModel { Title = "Proposed Core Reqirements - All chapters Hospital Accreditation Program" };            
            retVal.TableOfContents = LoadTableOfContent();
            return retVal;
        }

        private IEnumerable<TOCElementViewModel> LoadTableOfContent()
        {
            yield return new TOCElementViewModel { Key = "EC", Title = "Environment of Care (EC)" };
            yield return new TOCElementViewModel { Key = "EM", Title = "Emergency Management (EM)" };            
            yield return new TOCElementViewModel { Key = "HR", Title = "Human Resources (HR) "};
            yield return new TOCElementViewModel { Key = "IC", Title = "Infection Prevention and Control (IC)" };
            yield return new TOCElementViewModel { Key = "IM", Title = "Information Management (IM) " };
            yield return new TOCElementViewModel { Key = "LD", Title = "Leadership (LD) " };
            yield return new TOCElementViewModel { Key = "LS", Title = "Life Safety (LS)"};
            yield return new TOCElementViewModel { Key = "MM", Title = "Medication Management (MM) "};            
            yield return new TOCElementViewModel { Key = "PC", Title = "Provision of Care, Treatment, and Services (PC) "};            
            yield return new TOCElementViewModel { Key = "PC", Title = "Performance Improvement (PI)" };            
            yield return new TOCElementViewModel { Key = "RC", Title = "Record of Care, Treatment, and Services (RC) "};
            yield return new TOCElementViewModel { Key = "RI", Title = "Rights and Responsibilities of the Individual (RI)"};
            yield return new TOCElementViewModel { Key = "WT", Title = "Waived Testing (WT)"};            
        }

        public ActionResult Chapter(string chapterId)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(chapterId));

            var model = _standardManagementService.GetChapter(chapterId);
            //model.TableOfContents = new List<Models.TOCElementViewModel>();            
            return View(model);
        }


        public ActionResult StandardElement(string standardElementId)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(standardElementId));

            var model = _standardManagementService.GetStandardElement(standardElementId);            
            
            model.Key = standardElementId;            
            
            return View(model);
        }

        public ActionResult PerformanceElement(string standardElementId, string performanceItemId)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(standardElementId));
            Contract.Requires<ArgumentNullException>(!String.IsNullOrWhiteSpace(performanceItemId));

            var model = _standardManagementService.GetPerformanceElementViewModel(standardElementId, performanceItemId);            
            
            model.StandardId = standardElementId;            
            
            return View(model);
        }
    }
}