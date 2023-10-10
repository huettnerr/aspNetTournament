using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using ChemodartsWebApp.ViewModel;

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
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is object)
            {
                return View("Details", new TournamentViewModel() { T = t });
            }
            else
            {
                List<Tournament> ts = await _context.Tournaments.ToListAsync();
                return View(new TournamentViewModel() { Ts = ts });
            }
        }

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(int? tournamentId)
        {
            TournamentFactory tf = new TournamentFactory("Create", null);
            return View(new TournamentViewModel() {  TF = tf });
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(TournamentFactory factory)
        {
            Tournament? t = await _context.Tournaments.CreateWithFactory(ModelState, factory);
            if (t is object)
            {
                return RedirectToAction(nameof(Index), new { tournamentId = t.TournamentId });
            }

            return View(new TournamentViewModel() { TF = factory });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            TournamentFactory tf = new TournamentFactory("Edit", t);
            return View(new TournamentViewModel() { T = t, TF = tf });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, TournamentFactory tournamentFactory)
        {
            if (await _context.Tournaments.EditWithFactory(tournamentId, ModelState, tournamentFactory))
            {
                return RedirectToRoute("default", new { controller = "Home", action = "Tournaments" });
                return RedirectToAction(nameof(Index), new { tournamentId = tournamentId });
            }

            return View(new TournamentViewModel() { T = await _context.Tournaments.QueryId(tournamentId), TF = tournamentFactory });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            _context.Tournaments.Remove(t);
            await _context.SaveChangesAsync();

            return RedirectToRoute("default", new { controller = "Home", action = "Tournaments" });
        }
    }
}
