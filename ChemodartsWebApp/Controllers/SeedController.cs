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

            //var list = await _context.MapperRP.ToListAsync();
            //var seedsInRound = list.Where(rsp => rsp.TSP_RoundId == roundId).ToList();

            return View(new SeedViewModel(r.Seeds.OrderBy(s => s.SeedNr), r));
        }

        public async Task<IActionResult> Details(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            Seed? s = await _context.Seeds.QueryId(seedId);
            if (s is null) return NotFound();

            return View(new SeedViewModel(s, r));
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

        private async Task<MultiSelectList> getPlayersMultiSelectList(Round? r)
        {
            List<Player> Players = await _context.Players.ToListAsync();

            //Remove already subscribed Players
            List<Player>? SubscribedPlayers = r?.MappedSeedsPlayers.Where(msp => msp.Seed.Player is object).Select(msp => msp.Seed.Player).ToList();
            SubscribedPlayers?.ForEach(p => Players.Remove(p));

            return new MultiSelectList(Players.OrderBy(p => p.PlayerName), "PlayerId", "CombinedName", null);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddPlayersToSeed(int? tournamentId, int? roundId, int? seedId)
        {
            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            return View("Add", await getPlayersMultiSelectList(r));
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddPlayersToSeed(int? tournamentId, int? roundId, int? seedId, IFormCollection form)
        {
            ViewBag.YouSelected = form["Players"];
            string selectedValues = form["Players"];

            Round? r = await _context.Rounds.QueryId(roundId);
            if (r is null) return NotFound();

            if (!selectedValues?.Equals(String.Empty) ?? false)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                List<MapRoundSeedPlayer> availableMappedSeeds = r.MappedSeedsPlayers.Where(msp => msp.Seed.Player is null).ToList();

                List<int> playerIds = selectedValues.Split(",").Select(sV => Convert.ToInt32(sV)).ToList();

                //Update Seeds
                foreach (int playerId in playerIds)
                {
                    if (availableMappedSeeds.Count == 0)
                    {
                        sb.Append("Keine freien Seeds (mehr) gefunden\n");
                        break;
                    }

                    MapRoundSeedPlayer? msp = availableMappedSeeds.FirstOrDefault();

                    if (changeSeedPlayer(msp, playerId).Result)
                    {
                        availableMappedSeeds.Remove(msp);
                        sb.Append($"Spieler '{_context.Players.Single(p => p.PlayerId == playerId).CombinedName}' hat Seed #{msp.Seed.SeedNr}");
                    }
                    else
                    {
                        break;
                    }
                }

                ViewBag.Message = sb.ToString();
            }

            return View("Add", await getPlayersMultiSelectList(r));
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
