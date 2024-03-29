﻿using System;
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
    public class SettingsController : Controller
    {
        private readonly ChemodartsContext _context;
        private readonly ProgressionManager _progressionManager;

        private const string editQuery = "editMrpId";

        public SettingsController(ChemodartsContext context, ProgressionManager progressionManager)
        {
            _context = context;
            _progressionManager = progressionManager;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(int? tournamentId, int? roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is object)
            {
                return View(new SettingsViewModel() { R = r });
            }
            else
            {
                Tournament? t = await _context.Tournaments.QueryId(tournamentId);
                if (t is null) return NotFound();

                return View(new SettingsViewModel() { T = t });
            }
        }

        #region Progressions

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Progressions(int? tournamentId, int? roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            return View("Index", new SettingsViewModel() { R = r });
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> EditProgression(int? tournamentId, int? roundId, MapRoundProgression mrp)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            //Check if the progression setting has valid values
            ModelState.Remove($"{nameof(mrp)}.{nameof(mrp.BaseRound)}");
            if (ModelState.IsValid)
            {
                _context.MapperRP.Update(mrp);
                _context.Entry(mrp).Reference(m => m.TargetRound).Load();

                //try handle progression as a dummy 
                _progressionManager.UseDummySeeds = true;
                if (await _progressionManager.Manage(mrp))
                {
                    //everything all right, save in database
                    await _context.SaveChangesAsync();

                    ViewBag.UpdateMessage = "Save successful!";
                }
                else
                {
                    //error with progression handling
                    ViewBag.UpdateMessage = "Progression Setting has Errors!";
                }
            }
            else
            {
                //error with progression values
                ViewBag.UpdateMessage = ModelState.ToString();
            }

            return this.RedirectToPreviousPage(null, $"MRP_{mrp.TP_MrpMapId}", editQuery);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> CreateProgression(int? tournamentId, int? roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            //Add the Progression Rule
            MapRoundProgression newMrp = new MapRoundProgression() { TP_BaseRoundId = r.RoundId };
            _context.MapperRP.Add(newMrp);

            await _context.SaveChangesAsync();

            return this.RedirectToPreviousPage(new Dictionary<string, string>() {{ editQuery, newMrp.TP_MrpMapId.ToString() }}, $"MRP_{newMrp.TP_MrpMapId}");
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> DeleteProgression(int? tournamentId, int? roundId, int? mrpId)
        {
            MapRoundProgression? rp = await _context.MapperRP.QueryId(mrpId);
            if (rp is null) return NotFound();

            _context.MapperRP.Remove(rp);
            await _context.SaveChangesAsync();

            return this.RedirectToPreviousPage();
        }

        #endregion

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> StartRound(int? tournamentId, int roundId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            r.IsRoundStarted = true;
            r.IsRoundFinished = false;
            _context.Rounds.Update(r);

            if (r.Modus == RoundModus.RoundRobin)
            {
                RoundRobinLogic.UpdateSeedsInRound(r);
                RoundRobinLogic.UpdateMatchOrderInRound(r);
            }
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> FinishRound(int? tournamentId, int roundId)
        {
            //Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            //if (t is null) return NotFound();

            Round? r =await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            _progressionManager.UseDummySeeds = false;
            if(await _progressionManager.ManageAll(r))
            {

                r.IsRoundStarted = true;
                //r.IsRoundFinished = true;
                _context.Rounds.Update(r);
                await _context.SaveChangesAsync();

                int newRoundId = r.ProgressionRulesAsBase.FirstOrDefault()?.TargetRound?.RoundId ?? roundId;
                return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = newRoundId });
            }
            else
            {
                ViewBag.UpdateMessage = _progressionManager.ErrorMessage;
                return this.RedirectToPreviousPage();
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateSeeds(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = "RoundId ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            RoundRobinLogic.UpdateSeedsInRound(r);
            RoundRobinLogic.UpdateMatchOrderInRound(r); 
            await _context.SaveChangesAsync();

            return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RecreateMatches(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await _context.Tournaments.QueryId(tournamentId);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateMessage = $"RoundId {selectedRoundId} ungültig";
                return RedirectToAction(nameof(Index), t);
            }

            await RoundRobinLogic.RecreateAllMatches(_context, r);

            return RedirectToRoute("Match", new { controller = "Match", tournamentId = tournamentId, action = "Index", roundId = selectedRoundId });
        }

        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> UpdateKoFirstRound(int? tournamentId, int selectedRoundId, int selectedRound2Id)
        //{
        //    //KoFactory.UpdateFirstRoundSeeds(
        //    //    _context,
        //    //    new Round() { Groups = new List<Group>() { new Group(), new Group(), new Group(), new Group() } },
        //    //    new Group() { Matches = new List<Match>() { new Match(), new Match() } });

        //    Tournament? t = await _context.Tournaments.QueryId(tournamentId);
        //    if (t is null) return NotFound();

        //    Round? r1 = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
        //    Round? r2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
        //    if (r1 is null || r2 is null)
        //    {
        //        ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
        //        return RedirectToAction(nameof(Index), t);
        //    }

        //    RoundKoLogic.UpdateFirstRoundSeeds(_context, r1, r2.Groups.ElementAt(0));

        //    return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r2.RoundId });
        //}

        //[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> UpdateKoRound(int? tournamentId, int selectedRoundId)
        //{
        //    //Tournament? t = await queryId(tournamentId, _context.Tournaments);
        //    //if (t is null) return NotFound();

        //    //Group? g1 = t.Rounds.Where(r => r.Modus == Round.RoundModus.SingleKo).FirstOrDefault()?.Groups.Where(g => g.GroupId == finishedGroup).FirstOrDefault();
        //    ////Group? g2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
        //    //if (g1 is null || g2 is null)
        //    //{
        //    //    ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
        //    //    return View("TournamentSettings", t);
        //    //}

        //    Tournament? t = await _context.Tournaments.QueryId(tournamentId);
        //    if (t is null) return NotFound();

        //    Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
        //    if (r is null) return NotFound();

        //    RoundKoLogic.UpdateKoRoundSeeds(_context, r);

        //    return RedirectToRoute("Round", new { controller = "Round", tournamentId = tournamentId, action = "Index", roundId = r.RoundId });
        //}


    }
}
