using Atum.Domain.Surveillance;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class SurveillanceManagementController : Controller
    {
        //
        // GET: /SurveillanceManagement/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Schedules(int? i) 
        {

            return View();        
        }


        [HttpPost]
        public ActionResult Schedule(ScheduleViewModel model)
        {

            return View();
        }

        public ActionResult Schedule(int? index)
        {
            ScheduleViewModel model = new ScheduleViewModel(loadSchedule());
            
            
            return View(model);
        }



        public SurveillanceSchedule loadSchedule()
        {
            string title = "Schedule Title";
            SurveillanceSchedule retVal = new SurveillanceSchedule();
            retVal.Title = title;
            retVal.Frequency = Atum.Domain.Common.Frequency.BiMonthly;
            retVal.Survey = new Survey();
            retVal.Environment = new Atum.Domain.Common.Environment();

            return retVal;
        }


    }
}
