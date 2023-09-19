using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace MvcWebApplication1
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary AddRouteItem(this RouteValueDictionary rvd, string route, string value)
        {
            rvd.Add(route, value);
            return rvd;
        }
    }

    public static class IHtmlHelperExtension
    {
        public static IHtmlContent Link(this IHtmlHelper helper, 
            HttpContext context, 
            string linkText, 
            string route, 
            object value, 
            string? controller = null,
            string? action = null
        )
        {
            if (helper == null)
            {
                throw new ArgumentNullException(nameof(helper));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (linkText == null)
            {
                throw new ArgumentNullException(nameof(linkText));
            }

            var routeVals = new Dictionary<string, string> { { route, value?.ToString() } };
            context.Request.RouteValues.ToList().ForEach(rv => {
                if (!rv.Key.Equals(route)) routeVals.Add(rv.Key, rv.Value?.ToString());
            });

            return helper.ActionLink(linkText, action ?? context.Request.RouteValues["action"]?.ToString(), controller ?? context.Request.RouteValues["controller"]?.ToString(), routeVals);
        }
    }
}
