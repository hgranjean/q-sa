using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rules.Domain;
using Rules.WebEditor.Models;

namespace Rules.WebEditor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            
            var viewModel = GetJourney();

            return View(viewModel);
        }

        private static JourneyViewModel GetJourney()
        {
            var bladeViewModel = new BladeViewModel("Rule applications", BladeCategoryType.RuleApplication,
                                                    PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>());

            var viewModel = new JourneyViewModel(new List<BladeViewModel>(new[] {bladeViewModel}));

            return viewModel;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddBlade(string type, string id)
        {
            RuleObjectBase context = null;
            BladeViewModel bladeViewModel = null;
            if (type == "Entity")
            {
                context = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>().FirstOrDefault(item => item.Name == id);

                bladeViewModel = new BladeViewModel("Entities", BladeCategoryType.Entity,
                                               (context as RuleApplicationSpec).Entities.ToList<RuleObjectBase>());
            }
            else if (type == "RuleSet")
            {
                context = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>().FirstOrDefault();

                bladeViewModel = new BladeViewModel("RuleSets", BladeCategoryType.RuleSet,
                                               ((RuleApplicationSpec)context).Entities.FirstOrDefault(item => item.Name == id).RuleSets.ToList<RuleObjectBase>()); 
            }

            var viewModel = GetJourney();

            viewModel.Blades.Add(bladeViewModel);
            
            return View("Index",viewModel);
        }
    }
}
