using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Web.Mvc;

namespace SurveyWeb.Models
{
    public static class EnumUtils
    {
        public static string GetDisplayText(Type enumType, string member)
        {
            var value = enumType.GetMember(member);
            var attrs = value[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
            {
                if (attrs[0] is DescriptionAttribute)
                {
                    var display = attrs[0] as DescriptionAttribute;
                    return display.Description;
                }
            }
            return member as string;
        }

        public static IEnumerable GetEnumerableItems(Type enumType)
        {
            return Enum.GetNames(enumType).Select(x => new { Value = x, Text = EnumUtils.GetDisplayText(enumType, x) });
        }

        public static SelectList ToSelectList<T>(this T enumeration, string selected)
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                if (field.GetCustomAttributes(displayAttributeType, false).Count() > 0)
                {
                    DisplayAttribute attrs = (DisplayAttribute) field.
                                                                    GetCustomAttributes(displayAttributeType, false)
                                                                     .First();


                    items.Add(value, attrs.GetName());
                }

                else
                {
                    items.Add(value, value.ToString());
                }
            }

            return new SelectList(items, "Key", "Value", selected);
        }

        public static SelectList ToSelectList(this IEnumerable<string> enumeration)
        {
            return new SelectList(enumeration.Select(x => new SelectListItem {Text = x, Value = x})
                .ToList(), "Text", "Value");
        }
    }

}