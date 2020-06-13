using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace FileSharing.Helpers
{
    public static class CheckedRadioButton
    {
        public static MvcHtmlString CheckedRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value)
        {
            return CheckedRadioButtonFor(htmlHelper, expression, value, null);
        }

        public static MvcHtmlString CheckedRadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value, object htmlAttributes)
        {
            var func = expression.Compile();
            var attributes = new RouteValueDictionary(htmlAttributes);
            if ((object)func(htmlHelper.ViewData.Model) == value)
            {
                attributes["checked"] = "checked";
            }
            return htmlHelper.RadioButtonFor(expression, value, attributes);
        }
    }
}