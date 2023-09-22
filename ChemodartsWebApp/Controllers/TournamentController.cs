using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;

namespace ChemodartsWebApp.Controllers
{
    //https://learn.microsoft.com/de-de/aspnet/core/mvc/views/tag-helpers/built-in/anchor-tag-helper?view=aspnetcore-7.0
    public class TournamentController : Controller
    {

        private readonly ChemodartsContext _context;

        public TournamentController(ChemodartsContext context)
        {
            _context = context;
        }

        //GET Turnierübersicht
        public async Task<IActionResult> Index(int? tournamentId)
        {
            //search for spezific tournament
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            return View(t);
        }

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(int? tournamentId)
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("Name,StartTime")] TournamentFactory factory)
        {
            if (ModelState.IsValid)
            {
                Tournament? t = factory.CreateTournament();
                if(t is null) return NotFound();

                 _context.Tournaments.Add(t);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { tournamentId = t.TournamentId });
            }
            return View(factory);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId)
        {
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            _context.Tournaments.Remove(t);
            await _context.SaveChangesAsync();

            return RedirectToRoute("default", new { controller = "Home", action = "Tournaments" });
        }
    }
}
