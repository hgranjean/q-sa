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
        
        [Route("{type}/{ruleappid?}/{entityid?}/{rulesetid?}")]
        public ActionResult AddBlade(string type, string ruleappid, string entityid, string rulesetid)
        {
            RuleObjectBase context = null;
            BladeViewModel bladeViewModel = null;

            var viewModel = GetJourney();

            if (ruleappid != null)
            {
                context = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>().FirstOrDefault(item => String.Compare(item.Name, ruleappid, StringComparison.OrdinalIgnoreCase) == 0);

                bladeViewModel = new BladeViewModel("Entities", BladeCategoryType.Entity,
                                               ((RuleApplicationSpec)context).Entities.ToList<RuleObjectBase>());

                viewModel.Blades.Add(bladeViewModel);
            }
            if (entityid != null)
            {
                context = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>().FirstOrDefault(item => String.Compare(item.Name, ruleappid, StringComparison.OrdinalIgnoreCase) == 0);

                bladeViewModel = new BladeViewModel("RuleSets", BladeCategoryType.RuleSet,
                                               ((RuleApplicationSpec)context).Entities.FirstOrDefault(item => String.Compare(item.Name, entityid, StringComparison.OrdinalIgnoreCase) == 0).RuleSets.ToList<RuleObjectBase>());

                viewModel.Blades.Add(bladeViewModel);
            }
            if (rulesetid != null)
            {
                context = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>().FirstOrDefault(item => String.Compare(item.Name, ruleappid, StringComparison.OrdinalIgnoreCase) == 0);

                bladeViewModel = new BladeViewModel("Rules", BladeCategoryType.RuleSet,
                                               ((RuleApplicationSpec)context).Entities.FirstOrDefault(item => String.Compare(item.Name, entityid, StringComparison.OrdinalIgnoreCase) == 0)
                                               .RuleSets.FirstOrDefault(rs => String.Compare(rs.Name, rulesetid, StringComparison.OrdinalIgnoreCase) == 0).Actions.ToList<RuleObjectBase>());

                viewModel.Blades.Add(bladeViewModel);
            }
            
            return View("Index",viewModel);
        }
    }
}
