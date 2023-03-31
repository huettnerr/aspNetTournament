using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Xml.Linq;

namespace ChemodartsWebApp.Controllers
{
    //https://learn.microsoft.com/de-de/aspnet/core/mvc/views/tag-helpers/built-in/anchor-tag-helper?view=aspnetcore-7.0
    //[Route("/Tournament/Creaate", Name = "t_create")]
    //using --> <a asp-route="t_create">Create Tournament</a>

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

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsRecreateMatches(int? tournamentId, int selectedRoundId)
        {
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null)
            {
                ViewBag.UpdateSeedsMessage = "RoundId ungültig";
                return View("TournamentSettings", t);
            }

            //Delete all old matches
            IEnumerable<Match> oldMatches = await _context.Matches.Where(m => m.Group.Round.TournamentId == tournamentId).ToListAsync();
            _context.Matches.RemoveRange(oldMatches);

            List<Match> newMatches = MatchFactory.CreateMatches(r);
            _context.Matches.AddRange(newMatches);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Matches), new { id = tournamentId });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateKoFirstRound(int? tournamentId, int selectedRoundId, int selectedRound2Id)
        {
            //KoFactory.UpdateFirstRoundSeeds(
            //    _context,
            //    new Round() { Groups = new List<Group>() { new Group(), new Group(), new Group(), new Group() } },
            //    new Group() { Matches = new List<Match>() { new Match(), new Match() } });

            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r1 = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            Round? r2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            if (r1 is null || r2 is null)
            {
                ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
                return View("TournamentSettings", t);
            }

            KoFactory.UpdateFirstRoundSeeds(_context, r1, r2.Groups.ElementAt(0));

            return RedirectToAction(nameof(Round), new { id = selectedRound2Id });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> SettingsUpdateKoRound(int? tournamentId, int selectedRoundId)
        {
            //Tournament? t = await queryId(tournamentId, _context.Tournaments);
            //if (t is null) return NotFound();

            //Group? g1 = t.Rounds.Where(r => r.Modus == Round.RoundModus.SingleKo).FirstOrDefault()?.Groups.Where(g => g.GroupId == finishedGroup).FirstOrDefault();
            ////Group? g2 = t.Rounds.Where(x => x.RoundId == selectedRound2Id).FirstOrDefault();
            //if (g1 is null || g2 is null)
            //{
            //    ViewBag.UpdateKoFirstRoundMessage = "RoundId ungültig";
            //    return View("TournamentSettings", t);
            //}

            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            Round? r = t.Rounds.Where(x => x.RoundId == selectedRoundId).FirstOrDefault();
            if (r is null) return NotFound();

            KoFactory.UpdateKoRoundSeeds(_context, r);

            return RedirectToAction(nameof(Round), new { id = selectedRoundId });
        }

        #endregion

        #region Venues

        // GET Spielerübersicht
        public IActionResult Venues(int? tournamentId)
        {
            //search for spezific tournament
            Tournament? t = queryId(tournamentId, _context.Tournaments).Result;
            if (t is null) return NotFound();

            IEnumerable<Venue>? venues = t.Rounds.FirstOrDefault().MappedVenues.Select(x => x.Venue);
            List<Venue> v = _context.Venues.Where(v => v.VenueId > 0).ToList();
            if (venues is null) return NotFound();

            return View("TournamentVenues", venues);
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

            return new MultiSelectList(Players.OrderBy(p => p.PlayerName), "PlayerId", "CombinedName", null);
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

        #region Shuffle Seeds

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> ShufflePlayerSeeds(int? tournamentId)
        {
            Tournament? t = queryId(tournamentId, _context.Tournaments).Result;
            if (t is null) return NotFound();

            //Store all Players
            List<Player> players = t.MappedSeedsPlayers.Select(msp => msp.Player).OfType<Player>().ToList();

            //And clear old mapping
            foreach(MapTournamentSeedPlayer mps in t.MappedSeedsPlayers)
            {
                mps.TSP_PlayerId = null;
            }

            //Randomize
            int iSeed = 0;
            Random random = new Random();
            while(players.Count > 0)
            {
                Player randomPlayer = players.ElementAt(random.Next(players.Count));
                t.MappedSeedsPlayers.ElementAt(iSeed++).TSP_PlayerId = randomPlayer.PlayerId;
                players.Remove(randomPlayer);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Players), new { tournamentId = tournamentId });
        }

        #endregion

        #endregion

        #region Matches

        // GET Vollständige Matchliste
        public async Task<IActionResult> Matches(int? tournamentId, int? id, string? showAll)
        {
            //search for spezific tournament
            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null)
            {
                return NotFound();
            }

            var relevantRounds = t.Rounds.Where(r => r.Modus == Models.Round.RoundModus.RoundRobin).ToList();
            List<Match> matches = new List<Match>();
            relevantRounds.ForEach(r => matches.AddRange(getAllRoundMatches(r.RoundId)));

            matches = Match.OrderMatches(matches).ToList();

            if (matches.Count == 0) return NotFound();

            if (!showAll?.Equals("true") ?? true)
            {
                //Filter for active and not started matches
                matches = matches.Where(m => m.Status == Match.MatchStatus.Created || m.Status == Match.MatchStatus.Active).ToList();
            }

            return View("TournamentMatches", matches);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchStart(int? tournamentId, int? id, string? showAll)
        {
            Match? m = await queryId(id, _context.Matches);
            if (m is null) return NotFound();

            m.Status = Match.MatchStatus.Active;
            if (m.Score is null)
            {
                Score score = ScoreFactory.CreateScore(m);
                _context.Scores.Add(score);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{id}");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchAssignBoard(int? tournamentId, int? id, string? showAll)
        {
            Match? m = await queryId(id, _context.Matches);
            if (m is null) return NotFound();

            Tournament? t = await queryId(tournamentId, _context.Tournaments);
            if (t is null) return NotFound();

            m.Venue = m.Group.Round.MappedVenues.Select(mv => mv.Venue).Where(v => v.Match is null).FirstOrDefault();
            if (m.Venue is object)
            {
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.Message = "Kein freies Board gefunden";
            }

            return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{id}");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> MatchEditScore(int? tournamentId, int? id, string? showAll)
        {
            Match? m = await queryId(id, _context.Matches);
            //Match m = _context.DebugTournament.Rounds.ElementAt(0).Groups.ElementAt(0).Matches.ElementAt(0);
            if (m is null) return NotFound();

            return View("DisplayTemplates/Match/MatchEdit", m);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> MatchEditScore(Match m2, int? tournamentId, int? id, int? seed1Legs, int? seed2Legs, string? showAll)
        public async Task<IActionResult> MatchEditScore(int? tournamentId, int? id, int? seed1Legs, int? seed2Legs, Match.MatchStatus? newStatus, int? newVenueId, string? showAll)

        {
            Match? m = await queryId(id, _context.Matches);
            //Match m = _context.DebugTournament.Rounds.ElementAt(0).Groups.ElementAt(0).Matches.ElementAt(0);
            if (m is null) return NotFound();

            if (m.Score is object && seed1Legs is object && seed2Legs is object)
            {
                m.Score.P1Legs = seed1Legs ?? 0;
                m.Score.P2Legs = seed2Legs ?? 0;

                if (newStatus is object) { m.Status = newStatus; }
                //else { m.Status = Match.MatchStatus.Finished; }

                m.HandleNewStatus(m.Status);

                //if (newVenueId == -1) { 
                //    m.VenueId = null; 
                //} else { 
                //    m.Venue = newVenueId; 
                //}

                if (newVenueId?.Equals(0) ?? true) { m.VenueId = null; }
                else { m.Venue = await queryId(newVenueId, _context.Venues); }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Matches), "Tournament", new { tournamentId = tournamentId, showAll = showAll }, $"Match_{id}");
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

        #region Group Create (normal)

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
            return View("GroupCreate", groupFactory);
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

        #region Group Create Ko-System

        // GET: Tournament/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult GroupCreateKO(int? tournamentId, int? roundId)
        {
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GroupCreateKO(int? tournamentId, int? id, int? roundId, [Bind("NumberOfRounds,ThisRoundId")] KoFactory groupFactoryKo)
        {
            if (ModelState.IsValid)
            {
                Round? r = await queryId(groupFactoryKo.ThisRoundId, _context.Rounds);
                if (r is null) return NotFound();
                _context.Groups.RemoveRange(r.Groups);
                await _context.SaveChangesAsync();

                //Create Group
                if (!groupFactoryKo.CreateSystem(_context))
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Round), new { id = groupFactoryKo.ThisRoundId });
            }
            return View("GroupCreateKO", groupFactoryKo);
        }

        #endregion

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

        private IEnumerable<Match> getAllRoundMatches(int? roundId)
        {
            if (roundId is null) return Enumerable.Empty<Match>();

            return _context.Matches.Where(m => m.Group.RoundId == roundId).ToListAsync().Result;
        }
    }
}
