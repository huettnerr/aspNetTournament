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
            var roundsToTournament = _context.Rounds.Where(r => r.TournamentId == id).ToListAsync();
            return View(await roundsToTournament);
        }

        // GET: TournamentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TournamentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TournamentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TournamentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TournamentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TournamentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TournamentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
