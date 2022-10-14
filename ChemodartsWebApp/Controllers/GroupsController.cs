using ChemodartsWebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace ChemodartsWebApp.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ChemodartsContext _context;

        public GroupsController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: TournamentController
        public async Task<IActionResult> Index(int? id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            return View(group.MappedPlayers);
        }
    }
}
