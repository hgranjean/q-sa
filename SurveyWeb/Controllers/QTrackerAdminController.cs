using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SurveyWeb.Controllers
{
    public class QTrackerAdminController : Controller
    {
        // GET: QTrackerAdmin
        public ActionResult Dashboard()
        {
            return View();
        }
    
        public ActionResult Subscription()
        {
            return View();
        }
        
        public ActionResult Subscribers()
        {
            return View();
        }

        
        /// <summary>
        /// (AQS Business) Account associated with Subscriber - Billing History etc...
        /// </summary>
        /// <returns></returns>
        public ActionResult SubscriberAccount(int? subcriberId)
        {
            return View();
        }
        
        /// <summary>
        /// Root View of Subcriber's System (Facilities - Hospitals, Clinics etc..., Environments - Buildings, Floors, Organizational - Departments, User, Roles etc...
        /// </summary>
        /// <returns></returns>
        public ActionResult SubscriberSystem(int? subcriberId)
        {
            return View();
        }

        public ActionResult SubscriberUsers(int? subcriberId)
        {
            return View();
        }
    }

}