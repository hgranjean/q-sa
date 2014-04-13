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
            _ruleApplications.Add(new RuleApplicationSpec {Name = "app1", Entities = new List<EntitySpec>(new[]{new EntitySpec("Entity1", typeof(object))})});
            _ruleApplications.Add(new RuleApplicationSpec { Name = "app2", Entities = new List<EntitySpec>(new[] { new EntitySpec("Entity2", typeof(object)) })});
        }

        public static IEnumerable<RuleApplicationSpec> GetRuleApplications()
        {
            return _ruleApplications;
        }
    }
}