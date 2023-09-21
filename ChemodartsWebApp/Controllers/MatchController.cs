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

namespace ChemodartsWebApp.Controllers
{
    public class MatchController : Controller
    {
        private readonly ChemodartsContext _context;

        public MatchController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: Players/Details/5
        public async Task<IActionResult> MatchDetails(int? tournamentId, int? matchId)
        {
            //search for spezific tournament
            Match? m = await ControllerHelper.QueryId(matchId, _context.Matches);
            if (m is null)
            {
                return NotFound();
            }

            return View("DisplayTemplates/Match/MatchDetails", m);
        }

        // GET Vollständige Matchliste
        public async Task<IActionResult> Matches(int? tournamentId, int? id, string? showAll)
        {
            //search for spezific tournament
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            var relevantRounds = t.Rounds.Where(r => r.Modus == Models.Round.RoundModus.RoundRobin).ToList();
            List<Match> matches = new List<Match>();
            relevantRounds.ForEach(r => matches.AddRange(getAllRoundMatches(r.RoundId)));

            matches = Match.OrderMatches(matches).ToList();

            if (matches.Count == 0) return NotFound();

            if (!showAll?.Equals("true") ?? true)
            {
                //Filter for active and not started matches
                matches = matches.Where(m => m.Status == Match.MatchStatus.Created || m.Status == Match.MatchStatus.Active).ToList();
            }

            return View("TournamentMatches", matches);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchStart(int? tournamentId, int? id, string? showAll)
        {
            Match? m = await ControllerHelper.QueryId(id, _context.Matches);
            if (m is null) return NotFound();

            m.Status = Match.MatchStatus.Active;
            if (m.Score is null)
            {
                Score score = ScoreFactory.CreateScore(m);
                _context.Scores.Add(score);
            }

            await _context.SaveChangesAsync();

            if (m.Group.Round.Modus == Models.Round.RoundModus.RoundRobin)
            {
                return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{id}");
            }
            else
            {
                return RedirectToAction(nameof(Round), "Tournament", new { tournamentId = tournamentId, id = m.Group.Round.RoundId, showAll = showAll }, $"Match_{id}");
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchAssignBoard(int? tournamentId, int? id, string? showAll)
        {
            Match? m = await ControllerHelper.QueryId(id, _context.Matches);
            if (m is null) return NotFound();

            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            m.Venue = m.Group.Round.MappedVenues.Select(mv => mv.Venue).Where(v => v.Match is null).FirstOrDefault();
            if (m.Venue is object)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "Kein freies Board gefunden";
            }

            if (m.Group.Round.Modus == Models.Round.RoundModus.RoundRobin)
            {
                return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{id}");
            }
            else
            {
                return RedirectToAction(nameof(Round), "Tournament", new { tournamentId = tournamentId, id = m.Group.Round.RoundId, showAll = showAll }, $"Match_{id}");
            }
        }

        //[HttpGet]
        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> MatchEditScore(int? tournamentId, int? matchId, string? showAll)
        //{
        //    Match? m = await queryId(id, _context.Matches);
        //    //Match m = _context.DebugTournament.Rounds.ElementAt(0).Groups.ElementAt(0).Matches.ElementAt(0);
        //    if (m is null) return NotFound();

        //    return View("DisplayTemplates/Match/MatchEdit", m);
        //}

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchEditScore(int? tournamentId, int? matchId, int? seed1Legs, int? seed2Legs, Match.MatchStatus? newMatchStatus, int? newVenueId, string? showAll)

        {
            Match? m = await ControllerHelper.QueryId(matchId, _context.Matches);
            //Match m = _context.DebugTournament.Rounds.ElementAt(0).Groups.ElementAt(0).Matches.ElementAt(0);
            if (m is null) return NotFound();

            if (m.Score is object && seed1Legs is object && seed2Legs is object)
            {
                m.Score.P1Legs = seed1Legs ?? 0;
                m.Score.P2Legs = seed2Legs ?? 0;

                if (newMatchStatus is object) { m.Status = newMatchStatus; }
                //else { m.Status = Match.MatchStatus.Finished; }

                if (newVenueId?.Equals(0) ?? true) { m.VenueId = null; }
                m.Venue = await ControllerHelper.QueryId(newVenueId, _context.Venues);

                m.HandleNewStatus(m.Status);

                await _context.SaveChangesAsync();
            }

            try
            {
                return RedirectToPreviousPage("editId", "Match_");
            }
            catch
            {
                if (m.Group.Round.Modus == Models.Round.RoundModus.RoundRobin)
                {
                    return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{matchId}");
                }
                else
                {
                    return RedirectToAction(nameof(Round), "Tournament", new { tournamentId = tournamentId, id = m.Group.Round.RoundId, showAll = showAll }, $"Match_{matchId}");
                }
            }
        }

        private IEnumerable<Match> getAllRoundMatches(int? roundId)
        {
            if (roundId is null) return Enumerable.Empty<Match>();

            return _context.Matches.Where(m => m.Group.RoundId == roundId).ToListAsync().Result;
        }

        private RedirectResult RedirectToPreviousPage(string query, string fragment)
        {
            var uri = new System.Uri(HttpContext.Request.Headers.Referer);
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(uri.Query);
            var id = queryDictionary[query];
            UriBuilder uriB = new UriBuilder();
            uriB.Path = uri.AbsolutePath;
            if (id is object)
            {
                queryDictionary.Remove(query);
                uriB.Fragment = $"{fragment}{id}";
            }
            uriB.Query = queryDictionary.ToString();
            return Redirect(uriB.Uri.PathAndQuery + uriB.Fragment);
        }
    }
}
