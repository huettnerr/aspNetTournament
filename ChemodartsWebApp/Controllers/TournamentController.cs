﻿using Microsoft.AspNetCore.Http;
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

        //GET Turnierübersicht
        public async Task<IActionResult> Index(int? tournamentId)
        {
            if (tournamentId != null)
            {
                //search for spezific tournament
                Tournament? t = await queryId(tournamentId, _context.Tournaments);
                if (t is null)
                {
                    return NotFound();
                }
                else 
                { 
                    return View("TournamentOverview",t.Rounds);
                }
            }

            return View("Index", await _context.Tournaments.ToListAsync());
        }

        // GET Spielerübersicht
        public async Task<IActionResult> Players(int? tournamentId, int? id)
        {
             //search for spezific tournament
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            if (id is null)
            {
                return View("TournamentPlayers", t.MappedPlayers.Select(mp => mp.Player));
            }

            MapTournamentPlayer? p = t.MappedPlayers.Where(mp => mp.TPM_PlayerId == id).FirstOrDefault();

            if (p is null)
            {
                return NotFound();
            }
            else
            {
                return View("TournamentPlayerDetail", p);
            }
        }

        // GET Vollständige Matchliste
        public async Task<IActionResult> Matches(int? tournamentId)
        {
            //search for spezific tournament
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            IEnumerable<Match> matches = t.Rounds.SelectMany(r => r.Groups).Distinct().SelectMany(g => g.Matches).Distinct();
            //IEnumerable<Match> matches = await _context.Matches.Where(m => m.Group.Round.TournamentId == tournamentId).ToListAsync();

            return View("TournamentMatches", matches);
        }

        //GET Rundenansicht
        public async Task<IActionResult> Round(int? id)
        {
            //search for spezific tournament
            Round? r = await queryId(id, _context.Rounds);
            if (r is null)
            {
                return NotFound();
            }
            else
            {
                return View("RoundsOverview", r);
            }
        }

        // GET: Gruppenansicht
        public async Task<IActionResult> Group(int? id)
        {
            Group? g = await queryId(id, _context.Groups);
            if (g is null)
            {
                return NotFound();
            }
            else
            {
                return View("GroupOverview", g);
            }
        }

        //// GET: Matchansicht
        //public async Task<IActionResult> Matches(int? id)
        //{
        //    IEnumerable<Match> m = await _context.Matches.Where(m => m.Group.Round.TournamentId == id).ToListAsync();
        //    if (m is null)
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        return View("Matches/MatchesTable", m);
        //    }
        //}

        private ValueTask<T?> queryId<T>(int? id, DbSet<T> set) where T : class
        {
            if (id == null || set == null) return new ValueTask<T?>(result: null);

            return set.FindAsync(id);
        }
    }
}
