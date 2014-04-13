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

            ActionLinks = InitializeActionLinks();
        }

        private List<RouteValueDictionary> InitializeActionLinks()
        {
            return Items.Select(item => new RouteValueDictionary(new { Type = "Entity", Id = item.Name })).ToList();
        }
    }
}