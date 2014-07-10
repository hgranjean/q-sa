using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    [Authorize]
    public class StandardController : Controller
    {
        
        
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

        private Models.StandardDocumentViewModel loadDocument(int? id)
        {
            Models.StandardDocumentViewModel retVal = new Models.StandardDocumentViewModel();
            retVal.Title = "Proposed Core Reqirements - All chapters Long Term Care Accreditation Program";
            retVal.TableOfContents = loadTableOfContent();
            return retVal;
        }

        private List<Models.TOCElementViewModel> loadTableOfContent()
        {
            List<Models.TOCElementViewModel> retVal = new List<Models.TOCElementViewModel>();

            Models.TOCElementViewModel tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Environment of Care (EC)";
            tocElement.Key = "EC";
            retVal.Add(tocElement);

            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Emergency Management (EM) ";
            tocElement.Key = "EM";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Human Resources (HR) ";
            tocElement.Key = "HR";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Infection Prevention and Control (IC) ";
            tocElement.Key = "IC";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Information Management (IM) ";
            tocElement.Key = "IM";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Leadership (LD) ";
            tocElement.Key = "LD";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Life Safety (LS)";
            tocElement.Key = "LS";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Medication Management (MM) ";
            tocElement.Key = "MM";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Provision of Care, Treatment, and Services (PC) ";
            tocElement.Key = "PC";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Performance Improvement (PI) ";
            tocElement.Key = "PC";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Record of Care, Treatment, and Services (RC) ";
            tocElement.Key = "RC";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Rights and Responsibilities of the Individual (RI) ";
            tocElement.Key = "RI";
            retVal.Add(tocElement);
            
            tocElement = new Models.TOCElementViewModel();
            tocElement.Title = "Waived Testing (WT) ";
            tocElement.Key = "WT";
            retVal.Add(tocElement);
            

            return retVal;
        }


        public ActionResult Chapter(string chapterId)
        {
            Models.StandardDocumentViewModel model = new Models.StandardDocumentViewModel();
            model.TableOfContents = new List<Models.TOCElementViewModel>();
            model = Services.StandardsManagementServices.GetChapter(chapterId);
            return View(model);
        }


    }
}