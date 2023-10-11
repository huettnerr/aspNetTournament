using Microsoft.AspNetCore.Mvc;

namespace ChemodartsWebApp.Controllers
{
    public static class _ControllerExtensions
    {
        public static ActionResult RedirectToPreviousPage(this Controller c, Dictionary<string, string>? routeValues = null, string? fragment = null, string? editQueryStringToRemove = null)
        {
            //Uri uri = new Uri(HttpContext.Request.Headers.Referer);
            UriBuilder uriBuilder = new UriBuilder(c.HttpContext.Request.Headers.Referer);

            if (fragment is object)
            {
                uriBuilder.Fragment = fragment;
            }

            var queryDictionary = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);

            if (routeValues is object)
            {
                foreach(KeyValuePair<string, string> kvp in routeValues ) 
                {
                    queryDictionary.Add(kvp.Key, kvp.Value);
                }
            }

            if (editQueryStringToRemove is object)
            {
                var id = queryDictionary[editQueryStringToRemove];
                if (id is object)
                {
                    queryDictionary.Remove(editQueryStringToRemove);
                }
            }

            uriBuilder.Query = queryDictionary.ToString();
            return c.Redirect(uriBuilder.Uri.AbsoluteUri);
        }
    }
}
