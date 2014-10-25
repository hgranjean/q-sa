using Atum.Domain.Common;
using Atum.Utility.XML;
using Microsoft.Office.Interop.Word;
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


        //public ActionResult Standard(int? id)
        //{
        //    StandardViewModels model = new StandardViewModels();

        //    return View(model);        
        //}

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
            
            List<StandardType> modelStandardTypes = new List<StandardType>();
            modelStandardTypes.Add(new StandardType(1, "External Guidelines"));
            modelStandardTypes.Add(new StandardType(2, "Internal Policy"));

            
            model.StandardTypes = modelStandardTypes;
            

            LoadReferenceData(model);

            return View(model);
        }

        private void LoadReferenceData(StandardSearchViewModel model)
        {
            //model.StandardTypes = from value in Enumerable.Range(0, 2)
            //                          select value.ToString();
        }


        [HttpPost]
        public ActionResult Documents(StandardSearchViewModel model) 
        {
            //Get Search Criteria from model

            //Get Search Results and set in model

            //Return model
            
            
            return View(model);
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="standardElementId"></param>
        /// <returns></returns>
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