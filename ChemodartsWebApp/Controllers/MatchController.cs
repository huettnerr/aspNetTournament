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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ChemodartsWebApp.ViewModel;

namespace ChemodartsWebApp.Controllers
{
    public class MatchController : Controller
    {
        private readonly ChemodartsContext _context;

        private const string editQuery = "editMatchId";

        public MatchController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET Vollständige Matchliste
        public async Task<IActionResult> Index(int? tournamentId, int? roundId, int? matchId, string? showAll)
        {
            //search for spezific tournament
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null)
            {
                return NotFound();
            }

            List<Match> matches = new List<Match>();
            r.Groups.ToList().ForEach(g => matches.AddRange(g.Matches));

            if (!showAll?.Equals("true") ?? true)
            {
                //Filter for active and not started matches
                matches = matches.Where(m => m.Status == Match.MatchStatus.Created || m.Status == Match.MatchStatus.Active).ToList();
            }

            return View(new MatchViewModel() { R = r, Ms = matches.OrderBy(m => m.MatchOrderValue) });
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? tournamentId, int? roundId, int? matchId)
        {
            //search for spezific tournament
            Match? m = await _context.Matches.QueryId(matchId);
            if (m is null)
            {
                return NotFound();
            }

            return View(new MatchViewModel() { M = m });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Start(int? tournamentId, int? roundId, int? matchId)
        {
            Match? m = await _context.Matches.QueryId(matchId);
            if (m is null) return NotFound();

            m.SetNewStatus(Match.MatchStatus.Active);
            if (m.Score is null)
            {
                Score score = ScoreFactory.CreateScore(m);
                _context.Scores.Add(score);
            }

            await _context.SaveChangesAsync();

            return this.RedirectToPreviousPage(fragment: $"Match_{matchId}");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AssignBoard(int? tournamentId, int? roundId, int? matchId)
        {
            Match? m = await _context.Matches.QueryId(matchId);
            if (m is null) return NotFound();

            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
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

            return this.RedirectToPreviousPage(fragment: $"Match_{matchId}");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, int? matchId, int? seed1Legs, int? seed2Legs, Match.MatchStatus? newMatchStatus, int? newVenueId)

        {
            Match? m = await _context.Matches.QueryId(matchId);
            //Match m = _context.DebugTournament.Rounds.ElementAt(0).Groups.ElementAt(0).Matches.ElementAt(0);
            if (m is null) return NotFound();

            if (m.Score is object && seed1Legs is object && seed2Legs is object)
            {
                m.Score.P1Legs = seed1Legs ?? 0;
                m.Score.P2Legs = seed2Legs ?? 0;

                if (newMatchStatus is object) 
                {
                    m.SetNewStatus(newMatchStatus.Value);
                }

                if (newVenueId?.Equals(0) ?? true) { m.VenueId = null; }
                m.Venue = await _context.Venues.QueryId(newVenueId);

                _context.Matches.Update(m);
                await _context.SaveChangesAsync();
            }

            return this.RedirectToPreviousPage(fragment: $"Match_{matchId}", editQueryStringToRemove: editQuery);
        }
    }
}
