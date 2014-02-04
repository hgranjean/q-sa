using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace SurveyWeb.Models
{
    public class EnumUtils
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
    }
}