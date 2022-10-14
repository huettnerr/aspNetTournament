using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.Controllers
{
    public class MatchesController : Controller
    {
        private readonly ChemodartsContext _context;

        public MatchesController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: TournamentController
        public async Task<IActionResult> Index(int? id)
        {
            var matches = await _context.Matches.OrderByDescending(m => m.Status).ThenBy(m => m.TimeStarted).ToListAsync();
            if (matches == null)
            {
                return NotFound();
            }

            return View(matches);
        }
    }
}
