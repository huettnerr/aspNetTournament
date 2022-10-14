using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.Controllers
{
    public class RoundsController : Controller
    {
        private readonly ChemodartsContext _context;

        public RoundsController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: TournamentController
        public async Task<IActionResult> Index(int? id)
        {
            var round = await _context.Rounds.FindAsync(id);
            if (round == null)
            {
                return NotFound();
            }

            switch (round.Type)
            {
                case Models.Round.RoundType.RoundRobin:
                    return View("IndexRR", round.Groups);
                case Models.Round.RoundType.SingleKo:
                case Models.Round.RoundType.DoubleKo:
                    return View("IndexKO", round.Groups);
                default:
                    return View(round.Groups);
            }
        }
    }
}
