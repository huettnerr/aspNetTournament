using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ChemodartsWebApp.TagHelpers
{
    [HtmlTargetElement("abc", Attributes = ActionAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("a", Attributes = RouteValuesPrefix + "*")]
    public class RouteTagHelper : TagHelper
    {
        private const string ActionAttributeName = "cd-action";
        private const string RouteAttributeName = "cd-route";
        private const string RouteValuesDictionaryName = "cd-all-route-data";
        private const string RouteValuesPrefix = "cd-route-";
        private const string Href = "href";
        private IDictionary<string, string> _routeValues;

        [HtmlAttributeName(ActionAttributeName)]
        public string Action { get; set; }

        [HtmlAttributeName(RouteAttributeName)]
        public string RouteName { get; set; }

        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (_routeValues == null)
                {
                    _routeValues = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                }

                return _routeValues;
            }
            set
            {
                _routeValues = value;
            }
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        private IUrlHelper _urlHelper;

        public RouteTagHelper(IUrlHelperFactory urlHelperFactory, IActionContextAccessor contextAccessor)
        {
            _urlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (RouteName?.Equals(String.Empty) ?? true)
            {
                throw new ArgumentNullException(nameof(RouteName));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            ViewContext.RouteData.Values.ToList().ForEach(rd =>
            {
                if (rd.Key.Equals("controller")) return;
                else if (rd.Key.Equals("action")) return;

                RouteValues.Add(new KeyValuePair<string, string>(rd.Key.ToString(), rd.Value?.ToString() ?? ""));
            });

            if(Action is object)
            {
                if (RouteValues.ContainsKey("action")) RouteValues.Remove("action");

                RouteValues.Add(new KeyValuePair<string, string>("action", Action));
            }

            string url = _urlHelper.RouteUrl(RouteName, RouteValues);
            output.Attributes.SetAttribute("href", url);
        }
    }
}
