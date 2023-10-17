using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using static ChemodartsWebApp.ModelHelper.KoStage;

namespace ChemodartsWebApp.ModelHelper
{
    public class KoStage
    {
        public enum KoLinkingType
        {
            None,
            Winners,
            Losers,
            Mixed
        }

        public Round Round { get; set; }
        public int NumberOfStages { get; set; }
        public int StageNr { get; set; }
        public Group StageGroup { get; set; }
        public List<Match> StageMatches { get; set; }
        public KoLinkingType LinkingType { get; set; }
    }

    public class RoundKoLogic
    {
        public enum SeedingType
        {
            Random,
            FixedForAll,
            FixedForSome
        }

        public static KoStage? REF_STAGE_NULL = null;

        private readonly ChemodartsContext context;
        private readonly MatchLogic _matchLogic;

        public RoundKoLogic(ChemodartsContext dbContext, MatchLogic matchLogic)
        {
            context = dbContext;
            _matchLogic = matchLogic;
        }

        public async Task<bool> CreateKoSystem(Round r, int numberOfPlayers)
        {
            if (r is null || !(r.Modus == RoundModus.SingleKo || r.Modus == RoundModus.DoubleKo) || numberOfPlayers < 2) return false;

            //Remove old system if neccessary
            if(r.Groups is object && r.Groups.Count > 0)
            {
                context.Groups.RemoveRange(r.Groups);
                await context.SaveChangesAsync();
            }

            //2P -> 1; 4P -> 2, 8P -> 3, ...
            //Stage 1 is the final, stage 2 the semi and so on. NumberOfStages equals the first stage
            int numberOfStages = (int)Math.Ceiling(Math.Log2(numberOfPlayers));
            List<KoStage> stages = new List<KoStage>();    

            //now make first stage
            KoStage prevStage = new KoStage() { Round = r, NumberOfStages = numberOfStages };
            KoStage? prevLoserStage = null;
            KoStage? firstStage = createBracketStage(ref prevStage, numberOfStages, KoLinkingType.None, ref REF_STAGE_NULL);
            stages.Add(firstStage);

            //Only the first stage of the losers bracket will be entirely connected to the losing seeds
            prevLoserStage = createBracketStage(ref firstStage, numberOfStages - 1, KoLinkingType.Losers, ref REF_STAGE_NULL);
            prevLoserStage = adjustLoserStage(prevLoserStage, "Losers - First Round");
            stages.Add(prevLoserStage);

            prevStage = firstStage;
            for (int stageNr = numberOfStages - 1; stageNr > 0; stageNr--) 
            {
                KoStage winnerStage = createBracketStage(ref prevStage, stageNr, KoLinkingType.Winners, ref REF_STAGE_NULL);

                if (r.Modus == RoundModus.DoubleKo && numberOfStages >= 2)
                {
                    KoStage loserStage;

                    //Check if an intermediate losers round must be played
                    if(winnerStage.StageMatches.Count < prevLoserStage.StageMatches.Count)
                    {
                        //And let them play for a winner
                        loserStage = createBracketStage(ref prevLoserStage, stageNr, KoLinkingType.Winners, ref REF_STAGE_NULL);
                        loserStage = adjustLoserStage(loserStage, $"Losers - Pre {loserStage.StageGroup.GroupName}");
                        stages.Add(loserStage);

                        prevLoserStage = loserStage;
                    }

                    stages.Add(winnerStage);

                    //Flip matches of every second stage
                    if (stageNr - (numberOfStages - 1) % 2 != 0) winnerStage.StageMatches.Reverse();

                    //link a winner bracket loser with a loser bracket winner
                    loserStage = createBracketStage(ref winnerStage, stageNr, KoLinkingType.Mixed, ref prevLoserStage);
                    loserStage = adjustLoserStage(loserStage);
                    stages.Add(loserStage);

                    //Reverse back for winners bracket
                    if (stageNr - (numberOfStages - 1) % 2 != 0) winnerStage.StageMatches.Reverse();

                    prevLoserStage = loserStage;
                }
                else
                {
                    stages.Add(winnerStage);
                }

                prevStage = winnerStage;
            }
            KoStage winnersFinalStage = prevStage; //The last created Stage is the final
            KoStage? losersFinalStage = prevLoserStage; //The last created Stage is the final

            //when creating a double ko (with more than one stage) create the losing bracket
            if (r.Modus == RoundModus.DoubleKo && numberOfStages >= 2)
            {
                //Add final final of both brackets by creating a final stage
                KoStage finalFinalStage = new KoStage() { Round = r, NumberOfStages = 1 };
                finalFinalStage = createBracketStage(ref finalFinalStage, 1, KoLinkingType.None, ref prevLoserStage);

                //update all values of this final final
                finalFinalStage.StageGroup.GroupOrderValue = 1;
                finalFinalStage.StageGroup.GroupName = "Turnierfinale";
                Match finalFinal = finalFinalStage.StageMatches.First();
                finalFinal.MatchStage = 1;

                //Link the matches
                winnersFinalStage.StageMatches.First().WinnerFollowUpMatch = finalFinal;
                winnersFinalStage.StageMatches.First().WinnerFollowUpSeedNr = 1;
                losersFinalStage.StageMatches.First().WinnerFollowUpMatch = finalFinal;
                losersFinalStage.StageMatches.First().WinnerFollowUpSeedNr = 2;

                //Update the linked previous stages in the database
                context.Groups.Update(finalFinalStage.StageGroup);
                if (winnersFinalStage.StageMatches is object) context.Matches.UpdateRange(winnersFinalStage.StageMatches);
                if (losersFinalStage?.StageMatches is object) context.Matches.UpdateRange(losersFinalStage.StageMatches);
                context.SaveChangesAsync().Wait();

                stages.Add(finalFinalStage);
            }

            //adjust the stageOrder values
            stages.Reverse();
            int stageOrder = 1;
            foreach (var stage in stages) 
            {
                //adjust the group
                stage.StageGroup.GroupOrderValue = stageOrder;
                context.Groups.Update(stage.StageGroup);

                //Adjust match stages
                stage.StageMatches.ForEach(m => m.MatchStage = stageOrder);
                context.UpdateRange(stage.StageMatches);

                stageOrder++;
            }
            
            //save changes to database
            await context.SaveChangesAsync();

            return true;
        }

        private KoStage createBracketStage(ref KoStage previousStage, int stageNr, KoLinkingType linkingType, ref KoStage? previousLosersStage)
        {
            KoStage newStage = new KoStage() { 
                Round = previousStage.Round, 
                NumberOfStages = previousStage.NumberOfStages,
                StageNr = stageNr,
                StageMatches = new List<Match>()
            };
            
            int numberOfPlayersInStage = getPlayersPerStage(stageNr);

            //Make Group
            newStage.StageGroup = new Group()
            {
                RoundId = newStage.Round.RoundId,
                //GroupOrderValue = getStageOrderFromStage(newStage),
                GroupName = $"{getGroupName(numberOfPlayersInStage)}",
            };
            context.Groups.Add(newStage.StageGroup);
            context.SaveChangesAsync().Wait();

            //Make Matches
            int numberOfMatchesInStage = (linkingType != KoLinkingType.Mixed) ? numberOfPlayersInStage / 2 : previousStage.StageMatches.Count;
            int matchNr = 0;
            for (int matchStageNr = 0; matchStageNr < numberOfMatchesInStage; matchStageNr++)
            {
                Match m = new Match()
                {
                    GroupId = newStage.StageGroup.GroupId,
                    //MatchStage = getStageOrderFromStage(newStage), 
                    MatchOrderValue = ++matchNr,
                };
                newStage.StageMatches.Add(m);
            }
            context.Matches.AddRange(newStage.StageMatches);
            context.SaveChangesAsync().Wait(); //save to get matchIds

            //register as follow up if not base stage
            if (linkingType != KoLinkingType.None && previousStage.StageMatches is object)
            {
                for (int i = 0; i < newStage.StageMatches.Count; i++)
                {
                    if (linkingType == KoLinkingType.Winners && previousStage.StageMatches?.Count == 2 * newStage.StageMatches.Count)
                    {
                        previousStage.StageMatches[2 * i].WinnerFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i].WinnerFollowUpSeedNr = 1;

                        previousStage.StageMatches[2 * i + 1].WinnerFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i + 1].WinnerFollowUpSeedNr = 2;
                    }
                    else if (linkingType == KoLinkingType.Losers && previousStage.StageMatches?.Count == 2 * newStage.StageMatches.Count)
                    {
                        previousStage.StageMatches[2 * i].LoserFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i].LoserFollowUpSeedNr = 1;

                        previousStage.StageMatches[2 * i + 1].LoserFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i + 1].LoserFollowUpSeedNr = 2;
                    }
                    else if (linkingType == KoLinkingType.Mixed && previousLosersStage?.StageMatches is object 
                        && previousStage.StageMatches?.Count == newStage.StageMatches.Count
                        && previousLosersStage.StageMatches.Count == newStage.StageMatches.Count) 
                    {
                        previousLosersStage.StageMatches[i].WinnerFollowUpMatch = newStage.StageMatches[i];
                        previousLosersStage.StageMatches[i].WinnerFollowUpSeedNr = 1;

                        previousStage.StageMatches[i].LoserFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[i].LoserFollowUpSeedNr = 2;
                    }
                }

                //Update the linked previous stages in the database
                if(previousStage.StageMatches is object) context.Matches.UpdateRange(previousStage.StageMatches);
                if(previousLosersStage?.StageMatches is object) context.Matches.UpdateRange(previousLosersStage.StageMatches);
                context.SaveChangesAsync().Wait();
            }

            return newStage;
        } 

        private KoStage adjustLoserStage(KoStage loserStage, string? groupName = null)
        {
            //Change name of the group
            if (groupName is object) loserStage.StageGroup.GroupName = groupName;
            else loserStage.StageGroup.GroupName = $"Losers - {loserStage.StageGroup.GroupName}";

            return loserStage;
        }

        public string ErrorMessage;
        public async Task<bool> FillSeeds(SeedingType type, Round r, List<Seed>? seeds = null, int fixedSeedCount = 0)
        {
            //Get the matches of the first round of the tournament bracket
            List<Match>? firstRoundMatches = GetFirstRoundMatches(r);
            if (firstRoundMatches is null || firstRoundMatches.Count == 0)
            {
                ErrorMessage = $"No first round matches found!";
                return false;
            }
            int numberOfMatches = firstRoundMatches.Count;

            //Check if Seed List is valid, create dummys otherwise
            if (seeds is null || seeds.Count == 0)
            {
                //Use dummy names
                Group? firstRoundGroup = firstRoundMatches.FirstOrDefault()?.Group;
                if (firstRoundGroup is null) return false;

                seeds = new List<Seed>();
                int seedRank = 1;
                for (int seedNr = 0; seedNr < numberOfMatches * 2; seedNr++)
                {
                    Seed s = new Seed()
                    {
                        Group = firstRoundGroup,
                        SeedName = $"Seed {seedRank++}"
                    };
                    seeds.Add(s);
                }
                context.Seeds.AddRange(seeds);
                await context.SaveChangesAsync();
            }

            switch (type)
            {
                case SeedingType.Random:
                    //Randomize Seeds for KO-Round
                    firstRoundMatches = FillRandomizeSeedsForMatches(firstRoundMatches, seeds);
                    break;
                case SeedingType.FixedForAll:
                    firstRoundMatches = FillFixedSeedsForMatches(firstRoundMatches, seeds);
                    break;
                default:
                    return false;
            }

            context.Matches.UpdateRange(firstRoundMatches);
            await context.SaveChangesAsync();

            return true;
        }

        public List<Match>? GetFirstRoundMatches(Round r)
        {
            if (!(r.Modus == RoundModus.SingleKo || r.Modus == RoundModus.DoubleKo) || r.Groups is null || r.Groups.Count == 0) return null;

            return r.Groups.MaxBy(g => g.GroupOrderValue)?.Matches.ToList();
        }

        public Group? GetFirstRoundGroup(Round r)
        {
            return r.Groups?.MaxBy(g => g.GroupOrderValue);
        }

        public Match? GetFinal(Round r)
        {
            return r.Groups?.MinBy(g => g.GroupOrderValue ?? int.MaxValue)?.OrderedMatches.LastOrDefault();
        }

        public List<Seed> FillWithByeSeeds(List<Seed> seeds, int desiredSeedCount, int? groupId)
        {
            if (groupId is null) return seeds;

            while (seeds.Count < desiredSeedCount) seeds.Add(Seed.CreateByeSeed(groupId.Value));

            return seeds;
        }

        public List<Match> FillRandomizeSeedsForMatches(List<Match> matches, List<Seed> seeds)
        {
            Seed randomSeed;
            Random random = new Random();

            //Randomize 1st seed
            foreach (Match m in matches)
            {
                //Set seed 1
                randomSeed = seeds.ElementAt(random.Next(seeds.Count));
                m.Seed1 = randomSeed;
            }

            //Add bye seeds if necessary
            seeds = FillWithByeSeeds(seeds, matches.Count, matches.FirstOrDefault()?.GroupId);

            //Randomize 2nd seed
            foreach (Match m in matches)
            {
                //set seed 2
                randomSeed = seeds.ElementAt(random.Next(seeds.Count));
                seeds.Remove(randomSeed);
            }

            return matches;
        }

        public List<Match> FillFixedSeedsForMatches(List<Match> matches, List<Seed> seeds)
        {
            int numberOfMatches = matches.Count;
            seeds = FillWithByeSeeds(seeds, 2 * numberOfMatches, matches.FirstOrDefault()?.GroupId);

            //Pair the seeds top down so that seeds get matched with the lowest rank possible
            List<Tuple<int, int>>? pairsList = GetSeedPairsOfBracket(numberOfMatches);
            if (pairsList is null) return matches;

            //Fill matches based on List of pairs
            for (int i = 0; i < numberOfMatches; i++)
            {
                //move every second match in the other half of the bracket to ensure players of the same group meet as late as possible
                //int pairListPosition = (i % 2 == 0) ? i : (i + numberOfMatches / 2) % numberOfMatches;

                //fill match seeds according to list
                Match m = matches[i];
                m.Seed1 = seeds[pairsList[i].Item1 - 1];
                m.Seed2 = seeds[pairsList[i].Item2 - 1];
            }

            return matches;
        }

        public void UpdateKoRoundSeeds(Round r)
        {
            List<Match>? frm = GetFirstRoundMatches(r);
            if (frm is null) return;

            foreach(Match m in frm)
            {
                Match refM = m;
                _matchLogic.CheckWinnerAndHandleFollowUpMatches(ref refM, true);
                context.Matches.Update(refM);
            }

            context.SaveChanges();
        }

        #region Frontend Seed Dummys

        public void CreateDummySeedsForMatches(Round r)
        {
            List<Match> topTiers = new List<Match>();

            List<Match>? frm = GetFirstRoundMatches(r);
            if (frm is null) return;

            //Get all the Top level matches (without follow ups)
            foreach (Match m in frm)
            {
                getTopLevelMatches(m, ref topTiers);
            }

            topTiers.ForEach(tt => updateSeedNamesFromAncestors(tt));
        }

        private void getTopLevelMatches(Match? m, ref List<Match> topTiers)
        {
            if(m is null || topTiers is null) return;

            if (m.WinnerFollowUpMatch is null && m.LoserFollowUpMatch is null)
            {
                if(!topTiers.Contains(m)) topTiers.Add(m);
            }
            else
            {
                // propagate upwards
                getTopLevelMatches(m.WinnerFollowUpMatch, ref topTiers);
                getTopLevelMatches(m.LoserFollowUpMatch, ref topTiers);
            }
        }

        private void updateSeedNamesFromAncestors(Match m)
        {
            if (m.AncestorMatches?.Count == 2)
            {
                Match? am1 = m.AncestorMatches.Where(am => IsAncesterMatchForMatchsSeedNr(am, m, 1)).FirstOrDefault();
                if (am1 is object)
                {
                    updateSeedNamesFromAncestors(am1);

                    bool isWinnerAncestor = m.AncestorMatchesAsWinner?.Contains(am1) ?? false;
                    updateSeedNameFromAncestor(m.GroupId, am1, m.Seed1, out Seed? s1New, isWinnerAncestor);
                    m.Seed1 = s1New;
                }

                Match? am2 = m.AncestorMatches.Where(am => IsAncesterMatchForMatchsSeedNr(am, m, 2)).FirstOrDefault();
                if (am2 is object)
                {
                    updateSeedNamesFromAncestors(am2);

                    bool isWinnerAncestor = m.AncestorMatchesAsWinner?.Contains(am2) ?? false;
                    updateSeedNameFromAncestor(m.GroupId, am2, m.Seed2, out Seed? s2New, isWinnerAncestor);
                    m.Seed2 = s2New;
                }
            }
            else
            {
                //Seed has no ancesters, so add their name/player name to the list
                updateSeedName(m.Seed1);
                updateSeedName(m.Seed2);
            }
        }

        private bool IsAncesterMatchForMatchsSeedNr(Match ancesterMatch, Match match, int seedNr)
        {
            if ((ancesterMatch.WinnerFollowUpMatch?.Equals(match) ?? false) && ancesterMatch.WinnerFollowUpSeedNr == seedNr)
            {
                return true;
            }
            else if ((ancesterMatch.LoserFollowUpMatch?.Equals(match) ?? false) && ancesterMatch.LoserFollowUpSeedNr == seedNr)
            {
                return true;
            }
            else return false;
        }

        private void updateSeedNameFromAncestor(int groupId, Match ancestorMatch, Seed? S1orS2, out Seed? winnerOrLoserSeed, bool useWinnerSeed)
        {
            ancestorMatch.HandleWinnerLoserSeedOfMatch();

            if (ancestorMatch.WinnerSeed is object)
            {
                //Match has winner. There is only one possible seed for the next level.
                winnerOrLoserSeed = useWinnerSeed ? ancestorMatch.WinnerSeed : ancestorMatch.LoserSeed;
                updateSeedName(winnerOrLoserSeed);
            }
            else //Match has no winner
            {
                //check if this match already has a dummy seed and create if not
                winnerOrLoserSeed = S1orS2 is object ? S1orS2 : new Seed() { IsDummy = true, GroupId = groupId };

                //Update possible seeds
                List<Seed?> ps = new List<Seed?>();
                ps.AddRange(ancestorMatch.Seed1?.PossibleSeeds ?? new List<Seed?>());
                ps.AddRange(ancestorMatch.Seed2?.PossibleSeeds ?? new List<Seed?>());

                //update the name
                updateSeedName(winnerOrLoserSeed, ps);
            }
        }

        private void updateSeedName(Seed? s, List<Seed?>? ps = null)
        {
            if (s is null) return;

            if (ps is object)
            {
                //remove duplicates and null's
                s.PossibleSeeds = ps.Distinct().Where(s => s is object).ToList();
            }
            else
            {
                //only possible seed is seed itself
                s.PossibleSeeds = new List<Seed?>() { s };
            }

            //update the name
            if (s.Player is object)
            {
                s.SeedName = s.Player?.ShortName;
            }
            else if(s.IsByeSeed())
            {
                return;
                //s.SeedName = Seed.BYE_SEED.SeedName;
            }
            else if (s.PossibleSeeds.Count <= 12)
            {
                s.SeedName = String.Join(" | ", s.PossibleSeeds.Select(s => s.Player?.ShortName ?? s.SeedName));
            }
            else
            {
                s.SeedName = $"{s.PossibleSeeds.Count} possible seeds";
            }
        }

        #endregion

        private List<Tuple<int, int>>? GetSeedPairsOfBracket(int numberOfMatches)
        {
            // ensure number of matches is power of 2
            if (!(numberOfMatches > 0 && (numberOfMatches & (numberOfMatches - 1)) == 0)) return null;

            //Create a List of the seed Ranks for the matches
            List<Tuple<int, int>> pairsList = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2) };
            int rankCntInStage = 0;
            while (pairsList.Count < numberOfMatches)
            {
                //Split the old pairs
                List<int> ranksInPreviousStage = new List<int>();
                pairsList.ForEach(pair =>
                {
                    ranksInPreviousStage.Add(pair.Item1);
                    ranksInPreviousStage.Add(pair.Item2);
                });
                rankCntInStage = 2 * ranksInPreviousStage.Count;

                //combine with adjecent rank
                pairsList = new List<Tuple<int, int>>();
                ranksInPreviousStage.ForEach(rank =>
                {
                    pairsList.Add(new Tuple<int, int>(rank, rankCntInStage - (rank - 1)));
                });
            }

            return pairsList;
        }

        // Gets the number of matches per stage (stage 1 is final, stage 2 is semi and so on)
        private int getPlayersPerStage(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth));
        }

        private string getGroupName(int players)
        {
            switch (players)
            {
                case 2: return "Finale";
                case 4: return "Halbfinale";
                case 8: return "Viertelfinale";
                //case 16: return "Finale";
                default:
                    return $"Stufe der besten {players}";
            }
        }
    }
}
