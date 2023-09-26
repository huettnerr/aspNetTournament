﻿using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.Mvc.Routing;
using ChemodartsWebApp.Controllers;

namespace ChemodartsWebApp.TagHelpers
{
    [HtmlTargetElement("a", Attributes = ActionAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteAttributeName)]
    [HtmlTargetElement("a", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("a", Attributes = RouteValuesPrefix + "*")]
    [HtmlTargetElement("form", Attributes = ActionAttributeName)]
    [HtmlTargetElement("form", Attributes = RouteAttributeName)]
    [HtmlTargetElement("form", Attributes = RouteValuesDictionaryName)]
    [HtmlTargetElement("form", Attributes = RouteValuesPrefix + "*")]
    public class RouteTagHelper : TagHelper
    {
        private const string ActionAttributeName = "cd-action";
        private const string RouteAttributeName = "cd-route";
        private const string RouteValuesDictionaryName = "cd-all-route-data";
        private const string RouteValuesPrefix = "cd-route-";
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
                if (!IsRouteAttributeAllowed(RouteName, rd.Key)) return; 
                if (RouteValues.ContainsKey(rd.Key)) return;

                RouteValues.Add(new KeyValuePair<string, string>(rd.Key.ToString(), rd.Value?.ToString() ?? ""));
            });

            if(Action is object)
            {
                if (RouteValues.ContainsKey("action")) RouteValues.Remove("action");
                RouteValues.Add(new KeyValuePair<string, string>("action", Action));
            }

            string url = _urlHelper.RouteUrl(RouteName, RouteValues);

            if(context.TagName.Equals("a")) 
            {
                output.Attributes.SetAttribute("href", url);
            }
            else if(context.TagName.Equals("form"))
            {
                output.Attributes.SetAttribute("action", url);
                output.Attributes.SetAttribute("method", "post");
            }
            else
            {
                Console.WriteLine(context.TagName);
            }
        }


        private static Dictionary<string, List<string>> ALLOWED_ROUTE_ATTRIBUTES = new Dictionary<string, List<string>>()
        {
            { "Players", new List<string>() { "action", "playerId" } },
            { "Tournament", new List<string>() { "action", "tournamentId", "" } },
            { "Round", new List<string>() { "action", "tournamentId", "roundId" } },
            { "Seed", new List<string>() { "action", "tournamentId", "seedId" } },
            { "Settings", new List<string>() { "action", "tournamentId", "id" } },
            { "Group", new List<string>() { "action", "tournamentId", "roundId", "groupId" } },
            { "Match", new List<string>() { "action", "tournamentId", "roundId", "matchId", "showAll", "editMatchId" } },
            { "Venue", new List<string>() { "action", "tournamentId", "roundId", "venueId" } },
        };

        private static bool IsRouteAttributeAllowed(string routeName, string attributeName)
        {
            if (!ALLOWED_ROUTE_ATTRIBUTES.ContainsKey(routeName))
            {
                //Route has no restrictions 
                return true;
            }

            return ALLOWED_ROUTE_ATTRIBUTES[routeName].Contains(attributeName);
        }
    }
}
