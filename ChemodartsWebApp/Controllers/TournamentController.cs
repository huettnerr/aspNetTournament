using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.Controllers
{
    public class TournamentController : Controller
    {

        private readonly ChemodartsContext _context;

        public TournamentController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: TournamentController
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || _context.Tournaments == null) return NotFound();

            var rounds = await _context.Tournaments.FindAsync(id);
            if(rounds == null) return NotFound();

            return View(rounds.Rounds);
        }
    }
}
