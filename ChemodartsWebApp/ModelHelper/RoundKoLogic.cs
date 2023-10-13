using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.ModelHelper
{
    public static class RoundKoLogic
    {
        public enum SeedingType
        {
            Random,
            FixedForAll,
            FixedForSome
        }

        public static readonly Seed BYE_SEED = new Seed() { SeedName = "Bye" };

        public static async Task<bool> CreateKoSystem(ChemodartsContext context, Round r, int numberOfPlayers)
        {
            if (r is null || r.Modus != RoundModus.SingleKo || numberOfPlayers < 2) return false;

            //Remove old system
            context.Groups.RemoveRange(r.Groups);
            await context.SaveChangesAsync();

            //2P -> 1; 4P -> 2, 8P -> 3, ...
            int numberOfStages = (int)Math.Ceiling(Math.Log2(numberOfPlayers));

            //Stage 1 is the final, stage 2 the semi and so on
            List<Match>? prevStageMatches = null;
            int matchNr = 0;
            for (int stageNr = numberOfStages; stageNr > 0; stageNr--) 
            {
                int numberOfPlayersInStage = getPlayersPerStage(stageNr);

                //Make Group
                Group g = new Group()
                {
                    RoundId = r.RoundId,
                    GroupOrderValue = numberOfStages - stageNr,
                    GroupName = $"{getGroupName(numberOfPlayersInStage)} of \"{r.RoundName}\"",
                };
                context.Groups.Add(g);
                await context.SaveChangesAsync();

                //Make Matches
                int numberOfMatchesInStage = numberOfPlayersInStage / 2;
                List<Match> matches = new List<Match>();
                for (int matchStageNr = 0; matchStageNr < numberOfMatchesInStage; matchStageNr++)
                {
                    Match m = new Match()
                    {
                        GroupId = g.GroupId,
                        MatchStage = stageNr,
                        MatchOrderValue = ++matchNr,
                    };
                    matches.Add(m);
                }
                context.Matches.AddRange(matches);
                await context.SaveChangesAsync(); //save to get matchIds

                //register as follow up if not base stage
                if (prevStageMatches is object && prevStageMatches.Count == 2 * matches.Count)
                {
                    for(int i=0; i<matches.Count;i++)
                    {
                        prevStageMatches[2*i].WinnerFollowUpMatchId = matches[i].MatchId;
                        prevStageMatches[2*i + 1].WinnerFollowUpMatchId = matches[i].MatchId;
                    }
                    await context.SaveChangesAsync();
                }

                //store matches for next round
                prevStageMatches = matches;
            }

            //After creating this list should contain only the final. Updating it will recursivly update all matches
            prevStageMatches?.FirstOrDefault()?.Update();
            await context.SaveChangesAsync();

            return true;
        }

        public static string ErrorMessage;
        public static async Task<bool> FillSeeds(ChemodartsContext context, SeedingType type, Round r, List<Seed>? seeds = null, int fixedSeedCount = 0)
        {
            //Get the matches of the first round of the tournament bracket
            List<Match>? firstRoundMatches = RoundKoLogic.GetFirstRoundMatches(r);
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

        public static List<Match>? GetFirstRoundMatches(Round r)
        {
            if (!(r.Modus == RoundModus.SingleKo || r.Modus == RoundModus.DoubleKo) || r.Groups is null || r.Groups.Count == 0) return null;

            return r.Groups.MinBy(g => g.GroupOrderValue)?.Matches.ToList();
        }

        public static Group? GetFirstRoundGroup(Round r)
        {
            return r.Groups?.MinBy(g => g.GroupOrderValue);
        }

        public static List<Seed> FillWithByeSeeds(List<Seed> seeds, int desiredSeedCount)
        {
            while (seeds.Count < desiredSeedCount) seeds.Add(BYE_SEED);

            return seeds;
        }

        public static List<Match> FillRandomizeSeedsForMatches(List<Match> matches, List<Seed> seeds)
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
            seeds = FillWithByeSeeds(seeds, matches.Count);

            //Randomize 2nd seed
            foreach (Match m in matches)
            {
                //set seed 2
                randomSeed = seeds.ElementAt(random.Next(seeds.Count));
                seeds.Remove(randomSeed);

                //Handle bye seeds
                if (!randomSeed.Equals(BYE_SEED))
                {
                    m.Seed2 = randomSeed;
                }
                else
                {
                    m.Seed2 = null;
                    m.SetNewStatus(Match.MatchStatus.Finished);
                }
            }

            return matches;
        }

        public static List<Match> FillFixedSeedsForMatches(List<Match> matches, List<Seed> seeds)
        {
            int numberOfMatches = matches.Count;
            seeds = FillWithByeSeeds(seeds, 2 * numberOfMatches);

            //Pair the seeds top down so that seeds get matched with the lowest rank possible
            List<Tuple<int, int>>? pairsList = RoundKoLogic.GetSeedPairsOfBracket(numberOfMatches);
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

                //handle byes
                if (m.Seed1.Equals(RoundKoLogic.BYE_SEED))
                {
                    m.Seed1 = null;
                    m.SetNewStatus(Match.MatchStatus.Finished);
                }
                else if (m.Seed2.Equals(RoundKoLogic.BYE_SEED))
                {
                    m.Seed2 = null;
                    m.SetNewStatus(Match.MatchStatus.Finished);
                }
            }

            return matches;
        }

        public static void UpdateFirstRoundSeeds(Data.ChemodartsContext context, Round previousRound, Group firstKoRoundGroup)
        {
            int numberOfMatches = firstKoRoundGroup.Matches.Count;
            int matchNr = 0;
            foreach (Match m in firstKoRoundGroup.Matches)
            {
                if ((2 * numberOfMatches) == previousRound.Groups.Count)
                {
                    Seed sforS1 = previousRound.Groups.ElementAt(2 * matchNr).RankedSeeds.ElementAt(0);
                    m.Seed1Id = sforS1.SeedId;
                    m.Seed1.SeedName = sforS1.Player?.PlayerDartname ?? sforS1.SeedName;

                    Seed sforS2 = previousRound.Groups.ElementAt(2 * matchNr + 1).RankedSeeds.ElementAt(0);
                    m.Seed2Id = sforS2.SeedId;
                    m.Seed2.SeedName = sforS2.Player?.PlayerDartname ?? sforS2.SeedName;
                }
                else if (numberOfMatches == previousRound.Groups.Count)
                {
                    Seed sforS1 = previousRound.Groups.ElementAt(matchNr).RankedSeeds.ElementAt(0);
                    m.Seed1Id = sforS1.SeedId;
                    m.Seed1.SeedName = sforS1.Player?.PlayerDartname ?? sforS1.SeedName;

                    Seed sforS2 = previousRound.Groups.ElementAt(numberOfMatches - 1 - matchNr).RankedSeeds.ElementAt(1);
                    m.Seed2Id = sforS2.SeedId;
                    m.Seed2.SeedName = sforS2.Player?.PlayerDartname ?? sforS2.SeedName;
                }
                else
                {
                    //Group count does not match
                    return;
                }
                matchNr++;
            }

            context.SaveChanges();
        }

        public static void UpdateKoRoundSeeds(Data.ChemodartsContext context, Round r)
        {
            foreach(Group g in r.Groups)
            {
                foreach(Match m in g.Matches)
                {
                    m.Update();
                }
            }

            context.SaveChanges();
        }

        private static List<Tuple<int, int>>? GetSeedPairsOfBracket(int numberOfMatches)
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
        private static int getPlayersPerStage(int depth)
        {
            return Convert.ToInt32(Math.Pow(2, depth));
        }

        private static string getGroupName(int players)
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
