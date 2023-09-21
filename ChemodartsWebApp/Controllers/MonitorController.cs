using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ChemodartsWebApp.Data;

namespace ChemodartsWebApp.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _provider;

        public MonitorController(IActionDescriptorCollectionProvider provider)
        {
            _provider = provider;
        }

        //private readonly ChemodartsContext _context;

        //public MonitorController(ChemodartsContext context)
        //{
        //    _context = context;
        //}

        //GET Rundenansicht
        public async Task<IActionResult> Index(int? tournamentId, int? roundId)
        {
            return NotFound();
        }

        public IActionResult GetRoutes()
        {
            var routes = _provider.ActionDescriptors.Items.Select(x => new
            {
                Action = x.RouteValues["Action"],
                Controller = x.RouteValues["Controller"],
                Name = x.AttributeRouteInfo.Name,
                Template = x.AttributeRouteInfo.Template
            }).ToList();
            return Ok(routes);
        }
    }
}
