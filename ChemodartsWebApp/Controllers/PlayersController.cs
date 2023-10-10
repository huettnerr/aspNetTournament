using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using ChemodartsWebApp.ViewModel;

namespace ChemodartsWebApp.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ChemodartsContext _context;

        public PlayersController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(int? playerId)
        {
              return View(new PlayerViewModel() { Ps = await _context.Players.ToListAsync() });
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? playerId)
        {
            Player? p = await _context.Players.QueryId(playerId);
            if (p is null) return NotFound();

            return View(new PlayerViewModel() { P = p });
        }

        // GET: Players/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            PlayerFactory pf = new PlayerFactory("Create", null);
            return View(new PlayerViewModel() { PF = pf});
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayerFactory playerFactory)
        {
            Player? p = await _context.Players.CreateWithFactory(ModelState, playerFactory);
            if (p is object)
            {
                return RedirectToAction(nameof(Index), new { playerId = p.PlayerId });
            }

            return View(new PlayerViewModel() { PF = playerFactory });
        }

        // GET: Players/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? playerId)
        {
            Player? p = await _context.Players.QueryId(playerId);
            if (p is null) return NotFound();

            PlayerFactory pf = new PlayerFactory("Edit", p);
            return View(new PlayerViewModel() { PF = pf });
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? playerId, PlayerFactory playerFactory)
        {
            if(await _context.Players.EditWithFactory(playerId, ModelState, playerFactory))
            {
                return RedirectToAction(nameof(Index)/*, new { playerId = playerId }*/);
            }

            return View(new PlayerViewModel() { PF = playerFactory });
        }

        // GET: Players/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? playerId)
        {
            Player? p = await _context.Players.QueryId(playerId);
            if (p is null) return NotFound();

            return View(new PlayerViewModel() { P = p });
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int? playerId)
        {
            Player? p = await _context.Players.QueryId(playerId);
            if (p is null) return NotFound();

            _context.Players.Remove(p);            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
