using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

            BladeEditorViewModel bladeEditorViewModel = null;

            var viewModel = new JourneyViewModel(new List<BladeViewModel>(new[] {bladeViewModel}), bladeEditorViewModel);

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

        [Route("{type}/{ruleappid?}/{entityid?}/{rulesetid?}/{actionid?}", Order = 3)]
        public ActionResult AddBladeEditor(string type, string ruleappid, string entityid, string rulesetid, string actionid)
        {
            var viewModel = GetJourneyViewModel(ruleappid, entityid, rulesetid);

            var lastBlade = viewModel.Blades.Last();

            viewModel.BladeEditor = new BladeEditorViewModel(lastBlade);
           
            // Show content of the action
            return View("Index", viewModel);
        }

        [Route("{type}/{ruleappid?}/{entityid?}/{rulesetid?}", Order = 2)]
        public ActionResult AddBlade(string type, string ruleappid, string entityid, string rulesetid)
        {
            var viewModel = GetJourneyViewModel(ruleappid, entityid, rulesetid);

            return View("Index",viewModel);
        }

        private static JourneyViewModel GetJourneyViewModel(string ruleappid, string entityid, string rulesetid)
        {
            var ruleApplicationSpec =
                    (RuleApplicationSpec)PersistenceServices.GetRuleApplications()
                                       .ToList<RuleObjectBase>()
                                       .FirstOrDefault(
                                           item => String.Compare(item.Name, ruleappid, StringComparison.OrdinalIgnoreCase) == 0);
            
            var viewModel = GetJourney();

            if (ruleappid != null)
            {
                viewModel.Blades.Add(new BladeViewModel("Entities", BladeCategoryType.Entity,
                                                    ruleApplicationSpec.Entities.ToList<RuleObjectBase>()));
            }
            if (entityid != null)
            {
                viewModel.Blades.Add(new BladeViewModel("RuleSets", BladeCategoryType.RuleSet,
                                                    ruleApplicationSpec.Entities.FirstOrDefault(
                                                        item =>
                                                        String.Compare(item.Name, entityid, StringComparison.OrdinalIgnoreCase) ==
                                                        0).RuleSets.ToList<RuleObjectBase>()));
            }
            if (rulesetid != null)
            {
                viewModel.Blades.Add(new BladeViewModel("Rules", BladeCategoryType.Rules,
                                                    ruleApplicationSpec.Entities.FirstOrDefault(
                        item => String.Compare(item.Name, entityid, StringComparison.OrdinalIgnoreCase) == 0)
                                                   .RuleSets.FirstOrDefault(
                                                       rs =>
                                                       String.Compare(rs.Name, rulesetid, StringComparison.OrdinalIgnoreCase) ==
                                                       0).Actions.ToList<RuleObjectBase>()));
            }
            return viewModel;
        }

        [Route("AddSetValueAction")]
        public ActionResult AddSetValueAction()
        {
            var ruleapp1 = PersistenceServices.GetRuleApplications().ToList<RuleObjectBase>();
            (((RuleApplicationSpec)ruleapp1.FirstOrDefault()).Entities[0].RuleSets[0].Actions[0] as SimpleRuleSet).Rules.Add(new SetValueAction());

            var viewModel = GetJourney();
            
            return View("Index", viewModel);
        }
        
        [Route("Save")]
        public ActionResult Save()
        {
            PersistenceServices.SaveRuleApps();

            var viewModel = GetJourney();

            return View("Index", viewModel);
        }

        [Route("RuleApplication/GetMyData", Order = 1)]
        public JsonResult GetMyData()
        {
            var menu = new[] { "All egress doors are opening inside", "There is at least one efress door opening outside" };

            // new {title = "Cut", cmd = "cut", uiIcon = "ui-icon-scissors"}
            // Menu s = new SomeClass();
            // s.Property1 = "value";
            // s.Property2 = "another value";

            var str = new JavaScriptSerializer().Serialize(menu);

            return Json(str, JsonRequestBehavior.AllowGet); // need the AllowGet option to return data to a GET request
        }
    }
}
