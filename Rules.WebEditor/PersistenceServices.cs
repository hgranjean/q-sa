using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using Rules.Domain;

namespace Rules.WebEditor
{
    public class PersistenceServices
    {
        private static List<RuleApplicationSpec> _ruleApplications = null;

        static PersistenceServices()
        {
        }

        private static void CreateSampleApps()
        {
            var rules1 = new Rule[] {new SetValueAction() {Target = "Field1", Value = "1234"}};
            var action1 =
                new List<Rule>(new[] {new SimpleRuleSet {Name = "action1", Condition = "true", Rules = rules1.ToList()}});

            _ruleApplications.Add(new RuleApplicationSpec
                {
                    Name = "app1",
                    Entities =
                        new List<EntitySpec>(new[]
                            {
                                new EntitySpec("Entity1", typeof (object))
                                    {
                                        RuleSets =
                                            new List<RuleSpec>(new[] {new RuleSpec {Name = "RuleSet1", Actions = action1}})
                                    }
                            })
                });

            var action2 = new List<Rule>(new[] {new SimpleRuleSet {Name = "action2"}});

            _ruleApplications.Add(new RuleApplicationSpec
                {
                    Name = "app2",
                    Entities =
                        new List<EntitySpec>(new[]
                            {
                                new EntitySpec("Entity2", typeof (object))
                                    {
                                        RuleSets =
                                            new List<RuleSpec>(new[] {new RuleSpec {Name = "RuleSet2", Actions = action2}})
                                    }
                            })
                });
        }

        public static IEnumerable<RuleApplicationSpec> GetRuleApplications()
        {
            if (_ruleApplications == null)
            {
                LoadRuleApps();

                if (_ruleApplications.Any() == false)
                {
                    CreateSampleApps();
                }
            }

            return _ruleApplications;
        }

        public static void LoadRuleApps()
        {
            var ruleApps = new List<RuleApplicationSpec>();
            var destination = HttpContext.Current.Server.MapPath("~/App_Data");
            foreach (var fileName in Directory.GetFiles(destination, "*.rulespec"))
            {
                ruleApps.Add(RuleApplicationSpec.Load(fileName));
            }
            _ruleApplications = ruleApps;
        }

        public static void SaveRuleApps()
        {
            var destination = HttpContext.Current.Server.MapPath("~/App_Data");

            foreach (var ruleapp in _ruleApplications)
            {
                ruleapp.Save(Path.Combine(destination, ruleapp.Name + ".rulespec"));
            }
        }
    }
}