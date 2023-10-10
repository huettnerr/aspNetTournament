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
using System.Numerics;

namespace ChemodartsWebApp.Controllers
{
    public class VenueController : Controller
    {
        private readonly ChemodartsContext _context;

        public VenueController(ChemodartsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? tournamentId, int? roundId, int? venueId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            IEnumerable<Venue>? venues = r.MappedVenues.Select(x => x.Venue);
            if (venues is null) return NotFound();

            List<Venue> allUnmappedVenues = _context.Venues.ToListAsync().Result
                .Where(v => !v.MappedRounds.Any(mr => mr.RVM_RoundId == roundId)).ToList();
            ViewBag.UnmappedVenueList = allUnmappedVenues;

            return View(new VenueViewModel() { R = r, Vs = venues, UnmappedVs = allUnmappedVenues });
        }

        // GET Detailansicht
        public async Task<IActionResult> Details(int? tournamentId, int? roundId, int? venueId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            Venue? v = await _context.Venues.QueryId(venueId);
            if (v is null) return NotFound();

            return View(new VenueViewModel() { R = r, V = v});
        }

        [Authorize(Roles = "Administrator")]
        //[HttpPost]
        public async Task<IActionResult> Add(int? tournamentId, int? roundId, int? venueId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            Venue? v = await _context.Venues.QueryId(venueId);
            if (v is null) return NotFound();

            try
            {
                MapRoundVenue mrv = new MapRoundVenue()
                {
                    RVM_RoundId = r.RoundId,
                    RVM_VenueId = v.VenueId,
                };
                _context.MapperRV.Add(mrv);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { tournamentId = tournamentId, roundId = roundId });
        }

        // GET Detailansicht
        public async Task<IActionResult> Remove(int? tournamentId, int? roundId, int? venueId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            MapRoundVenue? mrv = r.MappedVenues.Where(r => r.RVM_VenueId == venueId).FirstOrDefault();
            if (mrv is null) return NotFound();

            try
            {
                _context.MapperRV.Remove(mrv);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index), new { tournamentId = tournamentId, roundId = roundId });
        }
    }
}
