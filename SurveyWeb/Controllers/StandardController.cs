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
        
        /// <summary>
        /// Controller Consturctor
        /// </summary>
        /// <param name="store"></param>
        /// <param name="standardManagementService"></param>
        public StandardController(ISurveyStore store, StandardsManagementServices standardManagementService)
        {
            _store = store;
            _standardManagementService = standardManagementService;
        }


        public ActionResult Standard(int? id)
        {
            StandardViewModels model = new StandardViewModels();


        
        }

        /// <summary>
        /// Standard Content
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, TOCElement>> GetTOCs()
        {
            return _standardManagementService.GetTOCs();
        }

        /// <summary>
        /// TODO: Replace with appropriate view model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Document(int? id)
        {
            var model = new StandardDocumentViewModel();
            if (id.HasValue)
            {
                //Load Document
                model = _standardManagementService.LoadDocument(id);
            }


            return View(model);
        }


        public ActionResult Documents()
        {
            var model = new StandardSearchViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Documents(StandardSearchViewModel model) 
        {
            //Get Search Criteria from model

            //Get Search Results and set in model

            //Return model
            
            
            return View(model);
        }
        //private StandardDocumentViewModel LoadDocument(int? id)
        //{
        //    //TODO: Load Document Title Form Store
        //    var retVal = new StandardDocumentViewModel { Title = "Proposed Core Reqirements - All chapters Hospital Accreditation Program" };            
        //    retVal.TableOfContents = LoadTableOfContent();
        //    return retVal;
        //}

        ////TODO: Move to aoppropriate store
        //private IEnumerable<TOCElementViewModel> LoadTableOfContent()
        //{
        //    yield return new TOCElementViewModel { Key = "EC", Title = "Environment of Care (EC)" };
        //    yield return new TOCElementViewModel { Key = "EM", Title = "Emergency Management (EM)" };            
        //    yield return new TOCElementViewModel { Key = "HR", Title = "Human Resources (HR) "};
        //    yield return new TOCElementViewModel { Key = "IC", Title = "Infection Prevention and Control (IC)" };
        //    yield return new TOCElementViewModel { Key = "IM", Title = "Information Management (IM) " };
        //    yield return new TOCElementViewModel { Key = "LD", Title = "Leadership (LD) " };
        //    yield return new TOCElementViewModel { Key = "LS", Title = "Life Safety (LS)"};
        //    yield return new TOCElementViewModel { Key = "MM", Title = "Medication Management (MM) "};            
        //    yield return new TOCElementViewModel { Key = "PC", Title = "Provision of Care, Treatment, and Services (PC) "};            
        //    yield return new TOCElementViewModel { Key = "PC", Title = "Performance Improvement (PI)" };            
        //    yield return new TOCElementViewModel { Key = "RC", Title = "Record of Care, Treatment, and Services (RC) "};
        //    yield return new TOCElementViewModel { Key = "RI", Title = "Rights and Responsibilities of the Individual (RI)"};
        //    yield return new TOCElementViewModel { Key = "WT", Title = "Waived Testing (WT)"};            
        //}

        /// <summary>
        /// Returns a standard chapter
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
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

            var chapterId = standardElementId.Split('.')[0];

            var model = _standardManagementService.GetPerformanceElementViewModel(chapterId, standardElementId, performanceItemId);            
            
            model.StandardId = standardElementId;            
            
            return View(model);
        }
    }

}