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
    //[Route("/Tournament/Creaate", Name = "t_create")]
    //using --> <a asp-route="t_create">Create Tournament</a>

    public class TournamentController : Controller
    {

        private readonly ChemodartsContext _context;

        public TournamentController(ChemodartsContext context)
        {
            _context = context;
        }

        #region Turnier

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

        #endregion

        #region Settings

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Settings(int? tournamentId)
        {
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            return View(t);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateSeeds(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Settings), t);
            }

            GroupFactory.UpdateSeeds(r.Groups);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsRecreateMatches(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Settings), t);
            }

            //Delete all old matches
            IEnumerable<Match> oldMatches = await _context.Matches.Where(m => m.Group.Round.TournamentId == tournamentId).ToListAsync();
            _context.Matches.RemoveRange(oldMatches);

            List<Match> newMatches = MatchFactory.CreateMatches(r);
            _context.Matches.AddRange(newMatches);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Match", new { controller = "Match", tournamentId = tournamentId, action = "Index", roundId = selectedRoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateKoFirstRound(int? tournamentId, int selectedRoundId, int selectedRound2Id)
        {
            //KoFactory.UpdateFirstRoundSeeds(
            //    _context,
            //    new Round() { Groups = new List<Group>() { new Group(), new Group(), new Group(), new Group() } },
            //    new Group() { Matches = new List<Match>() { new Match(), new Match() } });

            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r1 = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            Round? r2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            if (r1 is null || r2 is null)
            {
                ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Settings), t);
            }

            KoFactory.UpdateFirstRoundSeeds(_context, r1, r2.Groups.ElementAt(0));

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r2.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateKoRound(int? tournamentId, int selectedRoundId)
        {
            //Tournament? t = await queryId(tournamentId, _context.Tournaments);
            //if (t is null) return NotFound();

            //Group? g1 = t.Rounds.Where(r => r.Modus == Round.RoundModus.SingleKo).FirstOrDefault()?.Groups.Where(g => g.GroupId == finishedGroup).FirstOrDefault();
            ////Group? g2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            //if (g1 is null || g2 is null)
            //{
            //    ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
            //    return View("TournamentSettings", t);
            //}

            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null) return NotFound();

            KoFactory.UpdateKoRoundSeeds(_context, r);

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        #endregion
    }
}
