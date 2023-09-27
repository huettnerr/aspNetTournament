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

            return View(new SettingsViewModel(t));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateSeeds(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            GroupFactoryRR.UpdateSeeds(r.Groups);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsRecreateMatches(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = $"RoundId {selectedRoundId} ungültig";
                return RedirectToAction(nameof(Index), t);
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

            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r1 = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            Round? r2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            if (r1 is null || r2 is null)
            {
                ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            GroupFactoryKO.UpdateFirstRoundSeeds(_context, r1, r2.Groups.ElementAt(0));

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

            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null) return NotFound();

            GroupFactoryKO.UpdateKoRoundSeeds(_context, r);

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }


    }
}
