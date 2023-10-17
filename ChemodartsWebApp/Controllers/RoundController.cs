using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChemodartsWebApp.ViewModel;
using ChemodartsWebApp.ModelHelper;

namespace ChemodartsWebApp.Controllers
{
    public class RoundController : Controller
    {
        private readonly ChemodartsContext _context;
        private readonly RoundKoLogic _koLogic;

        public RoundController(ChemodartsContext context, RoundKoLogic koLogic)
        {
            _context = context;
            _koLogic = koLogic;
        }

        //GET Rundenansicht
        public async Task<IActionResult> Index(int? tournamentId, int? roundId)
        {
            //search for spezific tournament
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            return View(new RoundViewModel() { R = r });
        }

        // GET: Rounds/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if(t is null) return NotFound();

            RoundFactory rf = new RoundFactory("Create", t, null);
            return View(new RoundViewModel() { T = t, RF = rf });
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId, RoundFactory roundFactory)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            roundFactory.T = t;
            Round? r = await _context.Rounds.CreateWithFactory(ModelState, roundFactory);
            if (r is object)
            {
                return RedirectToRoute("Tournament", new { controller = "Tournament", tournamentId = tournamentId, action = "Index" });
                //return RedirectToAction(nameof(Index), new { roundId = r.RoundId });
            }

            return View(new RoundViewModel() { T = t, RF = roundFactory });
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateKO(int? tournamentId, int? roundId, GroupFactoryKO koFactory)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            List<Seed>? seeds = r.MappedSeedsPlayers?.Select(x => x.Seed).ToList();
            if (seeds is null || seeds.Count == 0) return NotFound();

            if (await _koLogic.CreateKoSystem(r, koFactory.NumberOfPlayers))
            {
                if (await _koLogic.FillSeeds(RoundKoLogic.SeedingType.Random, r, seeds))
                {
                    return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
                }
            }

            return View("Create", new GroupViewModel() { R = r, GF = koFactory });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            RoundFactory rf = new RoundFactory("Edit", r.Tournament, r);
            return View(new RoundViewModel() { T = r.Tournament, RF = rf });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, RoundFactory roundFactory)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            if (await _context.Rounds.EditWithFactory(roundId, ModelState, roundFactory))
            {
                return RedirectToRoute("Tournament", new { controller = "Tournament", tournamentId = tournamentId, action = "Index" });
                //return RedirectToAction(nameof(Index), new { roundId = r.RoundId });
            }

            return View(new RoundViewModel() { T = t, RF = roundFactory });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId, int? roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            _context.Rounds.Remove(r);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Tournament", new { controller = "Tournament", tournamentId = tournamentId, action = "Index"});
        }
    }
}
