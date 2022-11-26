using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ChemodartsWebApp.Controllers
{
    public class TournamentController : Controller
    {

        private readonly ChemodartsContext _context;

        public TournamentController(ChemodartsContext context)
        {
            _context = context;
        }

        #region Turnier anzeigen/erstellen

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

        #region Tournament Create

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult TournamentCreate(int? tournamentId)
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> TournamentCreate([Bind("Name,StartTime")] TournamentFactory factory)
        {
            if (ModelState.IsValid)
            {
                Tournament? t = factory.CreateTournament();
                if(t is null) return NotFound();

                 _context.Tournaments.Add(t);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index), t.TournamentId);
            }
            return View("TournamentCreate", factory);
        }

        #endregion

        #endregion

        #region Settings

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Settings(int? tournamentId)
        {
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            return View("TournamentSettings", t);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateSeeds(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = "RoundId ungültig";
                return View("TournamentSettings", t);
            }

            GroupFactory.UpdateSeeds(r.Groups);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Round), new { id = r.RoundId });
        }


        #endregion

        #region Players

        // GET Spielerübersicht
        public IActionResult Players(int? tournamentId, int? id)
        {
            //search for spezific tournament
            Tournament? t = queryId(tournamentId, _context.Tournaments).Result;
            if (t is null) return NotFound();

            if (id is null) return View("TournamentSeeds", t.Seeds.OrderBy(s => s.SeedNr));

            Seed? s = t.Seeds.Where(s => s.SeedId == id).FirstOrDefault();

            if (s is null) return NotFound();

            return View("TournamentSeedDetail", s);
        }

        #region Add Player

        private async Task<MultiSelectList> getPlayersMultiSelectList(Tournament? t)
        {
            List<Player> Players = await _context.Players.ToListAsync();

            //Remove already subscribed Players
            List<Player>? SubscribedPlayers = t?.MappedSeedsPlayers.Where(msp => msp.Seed.Player is object).Select(msp => msp.Seed.Player).ToList();
            SubscribedPlayers?.ForEach(p => Players.Remove(p));

            return new MultiSelectList(Players, "PlayerId", "CombinedName", null);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddPlayers(int? tournamentId, int? id)
        {
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            //Tournament t = _context.DebugTournament;
            if (t is null)
            {
                return NotFound();
            }

            ViewBag.Playerslist = getPlayersMultiSelectList(t).Result;

            return View("TournamentAddPlayers");
        }


        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddPlayers(int? tournamentId, int? id, IFormCollection form)
        {
            ViewBag.YouSelected = form["Players"];
            string selectedValues = form["Players"];

            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            //Tournament t = _context.DebugTournament;

            if (t is null)
            {
                return NotFound();
            }

            if (!selectedValues?.Equals(String.Empty) ?? false)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                List<MapTournamentSeedPlayer> availableMappedSeeds = t.MappedSeedsPlayers.Where(msp => msp.Seed.Player is null).ToList();

                List<int> playerIds = selectedValues.Split(",").Select(sV => Convert.ToInt32(sV)).ToList();

                //Update Seeds
                foreach (int playerId in playerIds)
                {
                    if (availableMappedSeeds.Count == 0) {
                        sb.Append("Keine freien Seeds (mehr) gefunden\n");
                        break;
                    }

                    MapTournamentSeedPlayer? msp = availableMappedSeeds.FirstOrDefault();

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

            return AddPlayers(tournamentId, id).Result;
        }

        #endregion

        #region CheckIn/Remove Player

        [Authorize(Roles = "Administrator")]
        public IActionResult PlayerRemove(int? tournamentId, int? id)
        {
            Tournament? t = queryId(tournamentId, _context.Tournaments).Result;
            if (t is null || id is null) return NotFound();

            MapTournamentSeedPlayer? msp = t.MappedSeedsPlayers.Where(msp => msp.Seed.SeedId == id).FirstOrDefault();

            if(changeSeedPlayer(msp, null).Result)
            {
                return Players(tournamentId, null);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PlayerCheckIn(int? tournamentId, int? id)
        {
            Tournament? t = queryId(tournamentId, _context.Tournaments).Result;
            if (t is null || id is null) return NotFound();

            MapTournamentSeedPlayer? msp = t.MappedSeedsPlayers.Where(msp => msp.Seed.SeedId == id).FirstOrDefault();
            if(msp is null) return NotFound();

            try
            {
                msp.TSP_PlayerCheckedIn = !msp.TSP_PlayerCheckedIn;
                _context.Update(msp);
                await _context.SaveChangesAsync();
                return Players(tournamentId, null);
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
        }

        #endregion

        #endregion

        #region Matches

        // GET Vollständige Matchliste
        public async Task<IActionResult> Matches(int? tournamentId)
        {
            //search for spezific tournament
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            //List<Match> matches = t.Rounds.SelectMany(r => r.Groups).Distinct().SelectMany(g => g.Matches).Distinct().ToList();
            //var matches = t.Rounds.FirstOrDefault().Groups.FirstOrDefault().Matches.ToList();
            IEnumerable<Match> matches = await _context.Matches.Where(m => m.Group.Round.TournamentId == tournamentId).ToListAsync();

            return View("TournamentMatches", matches);
        }

        #endregion

        #region Rounds

        //GET Rundenansicht
        public async Task<IActionResult> Round(int? tournamentId, int? id)
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

        // GET: Rounds/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult RoundCreate(int? tournamentId, int? id)
        {
            return View();
        }

        // POST: Rounds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RoundCreate(int? tournamentId, int? id, [Bind("Name,RoundModus")] RoundFactory roundFactory)
        {
            if (ModelState.IsValid)
            {
                Round? r = roundFactory.CreateRound(tournamentId);
                if (r is null)
                {
                    return NotFound();
                }

                _context.Rounds.Add(r);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Round), new { id = r.RoundId });
            }
            return View("RoundCreate", roundFactory);
        }

        #endregion

        #region Groups

        // GET: Gruppenansicht
        public async Task<IActionResult> Group(int? tournamentId, int? id)
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

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult GroupCreate(int? tournamentId, int? roundId)
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GroupCreate(int? tournamentId, int? id, int? roundId, [Bind("Name,PlayersPerGroup,RoundId")] GroupFactory groupFactory)
        {
            if (ModelState.IsValid)
            {
                //Create Group
                Group? g = groupFactory.CreateGroup();
                if (g is null)  return NotFound();

                _context.Groups.Add(g);
                await _context.SaveChangesAsync();

                //Create the seeds
                List<Seed>? seeds = groupFactory.CreateSeeds(g.GroupId);
                if (seeds is null) return NotFound();

                _context.Seeds.AddRange(seeds);
                await _context.SaveChangesAsync();

                //Map the seeds to the tournament
                List<MapTournamentSeedPlayer>? mappers = groupFactory.CreateMapping(tournamentId, seeds);
                if (mappers is null) return NotFound();

                _context.MapperTP.AddRange(mappers);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Group), new { id = g.GroupId });
            }
            return View("TournamentCreate", groupFactory);
        }

        public async Task<IActionResult> GroupDeleteSeed(int? tournamentId, int? id, int? seedId)
        {
            Group? g = await queryId(id, _context.Groups);
            if (g is null) return NotFound();

            Seed? s = g.Seeds.Where(s => s.SeedId == seedId).FirstOrDefault();
            if(s is null) return NotFound();

            g.Seeds.Remove(s);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Round), new { id = g.RoundId });
        }

        #endregion

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

        private async Task<bool> changeSeedPlayer(MapTournamentSeedPlayer? msp, int? playerId)
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

        private ValueTask<T?> queryId<T>(int? id, DbSet<T> set) where T : class
        {
            if (id == null || set == null) return new ValueTask<T?>(result: null);

            return set.FindAsync(id);
        }
    }
}
