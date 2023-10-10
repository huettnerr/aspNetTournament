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
                    //return View(new GroupViewModel(r, new OldGroupFactoryKO("CreateKO")));
                    return View(new GroupViewModel(r, new GroupFactoryKO("CreateKONew")));
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

            if (await RoundRobinLogic.CreateGroup(_context, rrFactory, r, ModelState))
            {
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
        public async Task<IActionResult> CreateKONew(int? tournamentId, int? roundId, int? groupId, GroupFactoryKO koFactory)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            if (await RoundKoLogic.CreateSystemSingleKO(_context, koFactory, r, ModelState))
            {
                return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
            }

            return View("Create", new GroupViewModel(r, koFactory));
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateKO(int? tournamentId, int? roundId, int? groupId, OldGroupFactoryKO koFactory)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            if (await RoundKoLogic.CreateSystem(_context, koFactory, r, ModelState))
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