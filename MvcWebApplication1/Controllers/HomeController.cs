﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MvcWebApplication1.Models;
using System.Diagnostics;

namespace MvcWebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private TestViewModel vm;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            vm = new TestViewModel();
        }

        public IActionResult Index(int? homeId, int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(int? homeId, int? id, TestViewModel tvm)
        {
            var d = HttpContext.Request.Headers.Referer;
            RouteValueDictionary ds = HttpContext.Request.RouteValues;

            var uri = new System.Uri(HttpContext.Request.Headers.Referer);
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var editId = queryDictionary["editId"];
            UriBuilder uriB = new UriBuilder();
            uriB.Path = uri.AbsolutePath;
            if(editId is object)
            {
                queryDictionary.Remove("editId");
                uriB.Fragment = $"id={editId}";
            }
            uriB.Query = queryDictionary.ToString();
            return Redirect(uriB.Uri.PathAndQuery+uriB.Fragment);
        }

        public IActionResult Privacy(int? homeId, int? id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? homeId, int? id)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}