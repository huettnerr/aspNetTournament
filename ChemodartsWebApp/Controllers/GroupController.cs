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
    public class GroupController : Controller
    {
        private readonly ChemodartsContext _context;

        public GroupController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: Gruppenansicht
        public async Task<IActionResult> Index(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await _context.Groups.QueryId(groupId);
            if (g is null)
            {
                return NotFound();
            }
            else
            {
                return View(new GroupViewModel(g));
            }
        }

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId, int? groupId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            switch(r.Modus)
            {
                case RoundModus.RoundRobin:
                    return View(new GroupViewModel(r, new GroupFactoryRR("CreateRR")));
                case RoundModus.SingleKo:
                    return View(new GroupViewModel(r, new GroupFactoryKO("CreateKO")));
                default:
                    return NotFound();
            }
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateRR(int? tournamentId, int? roundId, int? groupId, GroupFactoryRR rrFactory)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            rrFactory.R = r;
            Group? g = await _context.Groups.CreateWithFactory(ModelState, rrFactory);
            if (r is object)
            {
                //Create the seeds
                List<Seed>? seeds = rrFactory.CreateSeeds(g);
                if (seeds is null) return NotFound();

                _context.Seeds.AddRange(seeds);
                await _context.SaveChangesAsync();

                //Map the seeds to the tournament
                List<MapRoundSeedPlayer>? mappers = rrFactory.CreateMapping(r, seeds);
                if (mappers is null) return NotFound();

                _context.MapperRP.AddRange(mappers);
                await _context.SaveChangesAsync();

                return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
            }
            return View("Create", new GroupViewModel(r, rrFactory));
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateKO(int? tournamentId, int? roundId, int? groupId, GroupFactoryKO koFactory)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            koFactory.R = r;

            _context.Groups.RemoveRange(r.Groups);
            await _context.SaveChangesAsync();

            //Create Groups
            if (koFactory.CreateSystem(_context))
            {
                return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
            }

            return View("Create", new GroupViewModel(r, koFactory));
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await _context.Groups.QueryId(groupId);
            if (g is null) return NotFound();

            return View(new GroupViewModel(g, new GroupFactory("Edit", g)));
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, int? groupId, GroupFactory groupFactory)
        {
            Group? g = await _context.Groups.QueryId(groupId);
            if (g is null) return NotFound();

            if (await _context.Groups.EditWithFactory(groupId, ModelState, groupFactory))
            {
                return RedirectToRoute("Round", new { tournamentId = tournamentId, roundId = roundId, action = "Index" });
            }

            return View(new GroupViewModel(g, groupFactory));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await _context.Groups.QueryId(groupId);
            if (g is null) return NotFound();

            _context.Groups.Remove(g);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", action = "Index", tournamentId = tournamentId, roundId = roundId });
        }
    }
}