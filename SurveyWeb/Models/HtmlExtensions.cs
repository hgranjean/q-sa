using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace SurveyWeb.Models
{
    public static class HtmlHelperExtensions
    {
        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonListViewModel<TRadioButtonListValue>>> expression) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, null);
        }

        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonListViewModel<TRadioButtonListValue>>> expression, object htmlAttributes) where TModel : class
        {
            return htmlHelper.RadioButtonListFor(expression, new RouteValueDictionary(htmlAttributes));
        }

        public static string RadioButtonListFor<TModel, TRadioButtonListValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, RadioButtonListViewModel<TRadioButtonListValue>>> expression, IDictionary<string, object> htmlAttributes) where TModel : class
        {
            var inputName = GetInputName(expression);

            RadioButtonListViewModel<TRadioButtonListValue> radioButtonList = GetValue(htmlHelper, expression);

            if (radioButtonList == null)
                return String.Empty;

            if (radioButtonList.ListItems == null)
                return String.Empty;

            var divTag = new TagBuilder("div");
            divTag.MergeAttribute("id", inputName);
            divTag.MergeAttribute("class", "radio");
            foreach (var item in radioButtonList.ListItems)
            {
                var radioButtonTag = RadioButton(htmlHelper, inputName, new SelectListItem { Text = item.Text, Selected = item.Selected, Value = item.Value.ToString() }, htmlAttributes);

                divTag.InnerHtml += radioButtonTag;
            }

            return divTag.ToString() + htmlHelper.ValidationMessage(inputName, "*");
        }

        public static string GetInputName<TModel, TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            if (expression.Body.NodeType == ExpressionType.Call)
            {
                var methodCallExpression = (MethodCallExpression)expression.Body;
                string name = GetInputName(methodCallExpression);
                return name.Substring(expression.Parameters[0].Name.Length + 1);

            }
            return expression.Body.ToString().Substring(expression.Parameters[0].Name.Length + 1);
        }

        private static string GetInputName(MethodCallExpression expression)
        {
            // p => p.Foo.Bar().Baz.ToString() => p.Foo OR throw...

            var methodCallExpression = expression.Object as MethodCallExpression;
            if (methodCallExpression != null)
            {
                return GetInputName(methodCallExpression);
            }
            return expression.Object.ToString();
        }

        public static string RadioButton(this HtmlHelper htmlHelper, string name, SelectListItem listItem,
                             IDictionary<string, object> htmlAttributes)
        {
            var inputIdSb = new StringBuilder();
            inputIdSb.Append(name)
                .Append("_")
                .Append(listItem.Value);

            var sb = new StringBuilder();

            var builder = new TagBuilder("input");
            if (listItem.Selected) builder.MergeAttribute("checked", "checked");
            builder.MergeAttribute("type", "radio");
            builder.MergeAttribute("value", listItem.Value);
            builder.MergeAttribute("id", inputIdSb.ToString());
            builder.MergeAttribute("name", name + ".SelectedValue");
            builder.MergeAttributes(htmlAttributes);
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append(RadioButtonLabel(inputIdSb.ToString(), listItem.Text, htmlAttributes));
            sb.Append("<br>");

            return sb.ToString();
        }

        public static string RadioButtonLabel(string inputId, string displayText,
                                     IDictionary<string, object> htmlAttributes)
        {
            var labelBuilder = new TagBuilder("label");
            labelBuilder.MergeAttribute("for", inputId);
            labelBuilder.MergeAttributes(htmlAttributes);
            labelBuilder.InnerHtml = displayText;

            return labelBuilder.ToString(TagRenderMode.Normal);
        }


        public static TProperty GetValue<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression) where TModel : class
        {
            TModel model = htmlHelper.ViewData.Model;
            if (model == null)
            {
                return default(TProperty);
            }
            Func<TModel, TProperty> func = expression.Compile();
            return func(model);
        }
    }
}