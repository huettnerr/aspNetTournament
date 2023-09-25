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

namespace ChemodartsWebApp.Controllers
{
    public class RoundController : Controller
    {
        private readonly ChemodartsContext _context;

        public RoundController(ChemodartsContext context)
        {
            _context = context;
        }

        //GET Rundenansicht
        public async Task<IActionResult> Index(int? tournamentId, int? roundId)
        {
            //search for spezific tournament
            Round? r = await ControllerHelper.QueryId(roundId, _context.Rounds);
            if (r is null)
            {
                return NotFound();
            }
            else
            {
                return View(new RoundViewModel(r));
            }
        }

        // GET: Rounds/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId)
        {
            return View(new RoundViewModel(await ControllerHelper.QueryId(tournamentId, _context.Tournaments), null));
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId, [Bind("Name,RoundModus")] RoundFactory roundFactory)
        {
            Tournament? t = await ControllerHelper.QueryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            if (ModelState.IsValid )
            {
                Round? r = roundFactory.CreateRound(t);
                if (r is null)
                {
                    return NotFound();
                }

                _context.Rounds.Add(r);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { roundId = r.RoundId });
            }
            return View(new RoundViewModel(await ControllerHelper.QueryId(tournamentId, _context.Tournaments), roundFactory));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId, int? roundId)
        {
            Round? r = await ControllerHelper.QueryId(roundId, _context.Rounds);
            if (r is null) return NotFound();

            _context.Rounds.Remove(r);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Tournament", new { controller = "Tournament", tournamentId = tournamentId, action = "Index"});
        }
    }
}
