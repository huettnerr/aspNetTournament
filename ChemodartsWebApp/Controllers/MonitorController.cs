using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using ChemodartsWebApp.Data;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.Controllers
{
    public class MonitorController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _provider;
        private readonly ChemodartsContext _context;

        public MonitorController(IActionDescriptorCollectionProvider provider, ChemodartsContext context)
        {
            _provider = provider;
            _context = context;
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

        public async Task<IActionResult> FaultSeeds()
        {
            List<Seed> s = await _context.Seeds.ToListAsync();
            List<Group> g = await _context.Groups.ToListAsync();

            List<Seed> Wrong = s.Where(s => !g.Any(g => g.GroupId == s.GroupId)).ToList();

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
