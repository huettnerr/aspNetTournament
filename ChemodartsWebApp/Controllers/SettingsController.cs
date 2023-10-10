using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChemodartsWebApp.ViewModel;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.ModelHelper;

namespace ChemodartsWebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly ChemodartsContext _context;

        public SettingsController(ChemodartsContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(int? tournamentId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            return View(new SettingsViewModel() { T = t });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StartRound(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = "RoundId ungültig";
                return View(nameof(Index), new SettingsViewModel() { T = t });
            }

            r.IsRoundStarted = true;
            r.IsRoundFinished = false;
            _context.Rounds.Update(r);

            if (r.Modus == RoundModus.RoundRobin)
            {
                RoundRobinLogic.UpdateSeedsInRound(r);
                RoundRobinLogic.UpdateMatchOrderInRound(r);
            }
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> FinishRound(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = "RoundId ungültig";
                return View(nameof(Index), new SettingsViewModel() { T = t });
            }

            r.IsRoundStarted = true;
            r.IsRoundFinished = true;
            _context.Rounds.Update(r);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateSeeds(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            RoundRobinLogic.UpdateSeedsInRound(r);
            RoundRobinLogic.UpdateMatchOrderInRound(r); 
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RecreateMatches(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = $"RoundId {selectedRoundId} ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            await RoundRobinLogic.RecreateAllMatches(_context, r);

            return RedirectToRoute("Match", new { controller = "Match", tournamentId = tournamentId, action = "Index", roundId = selectedRoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateKoFirstRound(int? tournamentId, int selectedRoundId, int selectedRound2Id)
        {
            //KoFactory.UpdateFirstRoundSeeds(
            //    _context,
            //    new Round() { Groups = new List<Group>() { new Group(), new Group(), new Group(), new Group() } },
            //    new Group() { Matches = new List<Match>() { new Match(), new Match() } });

            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r1 = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            Round? r2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            if (r1 is null || r2 is null)
            {
                ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            RoundKoLogic.UpdateFirstRoundSeeds(_context, r1, r2.Groups.ElementAt(0));

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r2.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateKoRound(int? tournamentId, int selectedRoundId)
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

            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null) return NotFound();

            RoundKoLogic.UpdateKoRoundSeeds(_context, r);

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }


    }
}
