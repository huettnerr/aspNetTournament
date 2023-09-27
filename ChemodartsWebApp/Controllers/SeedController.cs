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
    public class SeedController : Controller
    {
        private readonly ChemodartsContext _context;

        public SeedController(ChemodartsContext context)
        {
            _context = context;
        }

        // GET Spielerübersicht
        public async Task<IActionResult> Index(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            return View(new SeedViewModel(r) {  Ss = r.Seeds.OrderBy(s => s.SeedNr) });
        }

        public async Task<IActionResult> Details(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            Seed? s = await _context.Seeds.QueryId(seedId);
            if (s is null) return NotFound();

            return View(new SeedViewModel(r) { S = s });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? tournamentId, int? roundId, int? seedId)
        {
            Seed? s = await _context.Seeds.QueryId(seedId);
            if (s is null) return NotFound();

            s.Group.Seeds.Remove(s);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #region Add Player

        private async Task<MultiSelectList?> getPlayersList(Round r)
        {
            if (r is null) return null;

            //Get all Players
            List<Player> Players = await _context.Players.ToListAsync();

            //Remove already subscribed Players
            List<Player?> SubscribedPlayers = r.MappedSeedsPlayers.Where(msp => msp.Seed.Player is object).Select(msp => msp.Seed.Player).ToList();
            SubscribedPlayers?.ForEach(p => Players.Remove(p));

            //Create the List
            return new MultiSelectList(Players.OrderBy(p => p.PlayerName), "PlayerId", "CombinedName", null);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddPlayers(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            //Return the View
            return View(new SeedViewModel(r) { Players = await getPlayersList(r), SelectedPlayerIds = new List<int>() });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> AddPlayers(int? tournamentId, int? roundId, int? seedId, SeedViewModel viewModel)
        {
            // Handle the selected player IDs (viewModel.SelectedPlayerIds)
            if (viewModel.SelectedPlayerIds != null && viewModel.SelectedPlayerIds.Any())
            {
                Round? r = await _context.Rounds.QueryId(roundId);
                if (r is null) return NotFound();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                List<MapRoundSeedPlayer> availableMappedSeeds = r.MappedSeedsPlayers.Where(msp => msp.Seed.Player is null).ToList();

                //Update Seeds
                foreach (int playerId in viewModel.SelectedPlayerIds)
                {
                    if (availableMappedSeeds.Count == 0)
                    {
                        sb.Append("Keine freien Seeds\n");
                        break;
                    }

                    MapRoundSeedPlayer? msp = availableMappedSeeds.FirstOrDefault();

                    if (changeSeedPlayer(msp, playerId).Result)
                    {
                        availableMappedSeeds.Remove(msp);
                        sb.Append($"Spieler '{_context.Players.Single(p => p.PlayerId == playerId).CombinedName}' hat Seed #{msp.Seed.SeedNr}\n");
                    }
                    else
                    {
                        break;
                    }
                }

                ViewBag.Message = sb.ToString();
                return View(new SeedViewModel(r) { Players = await getPlayersList(r), SelectedPlayerIds = new List<int>() });
            }

            // Redirect or return a view as needed
            return RedirectToAction(nameof(AddPlayers));
        }

        #endregion

        #region CheckIn/Remove Player

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RemovePlayerFromSeed(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            MapRoundSeedPlayer? msp = r.MappedSeedsPlayers.Where(msp => msp.Seed.SeedId == seedId).FirstOrDefault();

            if (changeSeedPlayer(msp, null).Result)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CheckIn(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            MapRoundSeedPlayer? msp = r.MappedSeedsPlayers.Where(msp => msp.Seed.SeedId == seedId).FirstOrDefault();
            if (msp is null) return NotFound();

            try
            {
                msp.TSP_PlayerCheckedIn = !msp.TSP_PlayerCheckedIn;
                _context.Update(msp);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        private async Task<bool> changeSeedPlayer(MapRoundSeedPlayer? msp, int? playerId)
        {
            if (msp is null) return false;

            try
            {
                msp.TSP_PlayerId = playerId;
                _context.Update(msp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        #endregion

        #region Shuffle Seeds

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ShuffleSeeds(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            //Store all Players
            List<Player> players = r.MappedSeedsPlayers.Select(msp => msp.Player).OfType<Player>().ToList();

            //And clear old mapping
            foreach (MapRoundSeedPlayer mps in r.MappedSeedsPlayers)
            {
                mps.TSP_PlayerId = null;
            }

            //Randomize
            int iSeed = 0;
            Random random = new Random();
            while (players.Count > 0)
            {
                Player randomPlayer = players.ElementAt(random.Next(players.Count));
                r.MappedSeedsPlayers.ElementAt(iSeed++).TSP_PlayerId = randomPlayer.PlayerId;
                players.Remove(randomPlayer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
