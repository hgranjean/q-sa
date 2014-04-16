using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rules.Domain;

namespace Rules.WebEditor
{
    public class PersistenceServices
    {
        private static List<RuleApplicationSpec> _ruleApplications = new List<RuleApplicationSpec>();

        static PersistenceServices()
        {
            var action1 = new List<Rule>(new[] {new SimpleRuleSet {Name = "action1"}});

            _ruleApplications.Add(new RuleApplicationSpec
                {
                    Name = "app1",
                    Entities =
                        new List<EntitySpec>(new[]
                            {
                                new EntitySpec("Entity1", typeof (object))
                                    {
                                        RuleSets = new List<RuleSpecification>(new []{new RuleSpecification{ Name = "RuleSet1", Actions = action1}})
                                    }
                            })
                });

            var action2 = new List<Rule>(new[] { new SimpleRuleSet { Name = "action2" } });
            
            _ruleApplications.Add(new RuleApplicationSpec
                {
                    Name = "app2",
                    Entities =
                        new List<EntitySpec>(new[]
                            {
                                new EntitySpec("Entity2", typeof (object))
                                    {
                                        RuleSets = new List<RuleSpecification>(new []{new RuleSpecification{ Name = "RuleSet2", Actions = action2}})
                                    }
                            })
                });
        }

        public static IEnumerable<RuleApplicationSpec> GetRuleApplications()
        {
            return _ruleApplications;
        }
    }
}