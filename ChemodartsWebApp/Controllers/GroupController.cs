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
            Group? g = await ControllerHelper.QueryId(groupId, _context.Groups);
            if (g is null)
            {
                return NotFound();
            }
            else
            {
                return View(g);
            }
        }

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(int? tournamentId, int? roundId, int? groupId)
        {
            Round? r = await ControllerHelper.QueryId(roundId, _context.Rounds);
            if (r is null) return NotFound();

            switch(r.Modus)
            {
                case RoundModus.RoundRobin:
                    return View("Create");
                case RoundModus.SingleKo:
                    return View("CreateKO");
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
        public async Task<IActionResult> Create(int? tournamentId, int? roundId, int? groupId, [Bind("Name,PlayersPerGroup,RoundId")] GroupFactory groupFactory)
        {
            Round? r = await ControllerHelper.QueryId(roundId, _context.Rounds);
            if (r is null) return NotFound();

            if (ModelState.IsValid)
            {
                //Create Group
                Group? g = groupFactory.CreateGroup(r);
                if (g is null) return NotFound();

                _context.Groups.Add(g);
                await _context.SaveChangesAsync();

                //Create the seeds
                List<Seed>? seeds = groupFactory.CreateSeeds(g);
                if (seeds is null) return NotFound();

                _context.Seeds.AddRange(seeds);
                await _context.SaveChangesAsync();

                //Map the seeds to the tournament
                List<MapTournamentSeedPlayer>? mappers = groupFactory.CreateMapping(tournamentId, seeds);
                if (mappers is null) return NotFound();

                _context.MapperTP.AddRange(mappers);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new { groupId = g.GroupId });
            }
            return View(groupFactory);
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateKO(int? tournamentId, int? roundId, int? groupId, [Bind("NumberOfRounds,ThisRoundId")] KoFactory groupFactoryKo)
        {
            Round? r = await ControllerHelper.QueryId(roundId, _context.Rounds);
            if (r is null) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Groups.RemoveRange(r.Groups);
                await _context.SaveChangesAsync();

                //Create Group
                if (!groupFactoryKo.CreateSystem(r, _context))
                {
                    return NotFound();
                }
                return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
            }
            return View(groupFactoryKo);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await ControllerHelper.QueryId(groupId, _context.Groups);
            if (g is null) return NotFound();

            _context.Groups.Remove(g);
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", action = "Index", tournamentId = tournamentId, roundId = roundId });
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, int? groupId)
        {
            Group? g = await ControllerHelper.QueryId(groupId, _context.Groups);
            if (g is null) return NotFound();

            return View(g);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? tournamentId, int? roundId, int? groupId, [Bind("GroupId,GroupName")] Group group)
        {
            if (groupId != group.GroupId)
            {
                return RedirectToAction(nameof(Edit));
            }

            try
            {
                Group? g = await ControllerHelper.QueryId(groupId, _context.Groups);
                if (g is null) return NotFound();

                g.GroupName = group.GroupName;

                //_context.Groups.
                _context.Update(g);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), new {groupId = g.GroupId });
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }
    }
}