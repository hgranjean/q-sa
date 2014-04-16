using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Rules.Domain;

namespace Rules.WebEditor.Models
{
    public enum BladeCategoryType
    {
        RuleApplication,
        Entity,
        RuleSet
    }

    public class BladeViewModel
    {
        public string Name { get; set; }
        public BladeCategoryType CategoryType { get; set; }
        public List<RuleObjectBase> Items { get; set; }
        public List<RouteValueDictionary> ActionLinks { get; set; }

        public BladeViewModel(string name, BladeCategoryType categoryType, List<RuleObjectBase> items)
        {
            Name = name;
            CategoryType = categoryType;
            Items = items;

            ActionLinks = GetActionLinks();
        }

        private List<RouteValueDictionary> GetActionLinks()
        {   
            switch (CategoryType)
            {
                case BladeCategoryType.RuleApplication:
                    return Items.Select(item => new RouteValueDictionary(new { type = "entity", ruleappid = item.Name })).ToList();
                case BladeCategoryType.Entity:
                    return Items.Select(item => new RouteValueDictionary(new { type = "ruleset", ruleappid = "app1", entityid = item.Name })).ToList();
                case BladeCategoryType.RuleSet:
                    return Items.Select(item => new RouteValueDictionary(new { type = "rules", ruleappid = "app1", entityid = "entity1", rulesetid = item.Name })).ToList();
            }

            return null;

        }
    }
}