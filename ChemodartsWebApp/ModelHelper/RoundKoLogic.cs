using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace ChemodartsWebApp.ModelHelper
{
    public class KoStage
    {
        public Round Round { get; set; }
        public int NumberOfStages { get; set; }
        public int StageNr { get; set; }
        public Group StageGroup { get; set; }
        public List<Match> StageMatches { get; set; }
    }

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

            //now make first stage
            KoStage? firstStage = await createBracketStage(context, new KoStage() { Round = r, NumberOfStages = numberOfStages}, numberOfStages, null);

            KoStage prevStage = firstStage;
            for (int stageNr = numberOfStages - 1; stageNr > 0; stageNr--) 
            {
                prevStage = await createBracketStage(context, prevStage, stageNr, true);
            }
            KoStage winnersFinalStage = prevStage; //The last created Stage is the final
            //winnersFinalStage.StageMatches?.FirstOrDefault()?.Update();

            //when creating a double ko (with more than one stage) create the losing bracket
            if (r.Modus == RoundModus.DoubleKo && numberOfStages >= 2)
            {
                //add following stages
                prevStage = firstStage;
                for (int stageNr = numberOfStages - 1; stageNr > 0; stageNr--)
                {
                    bool isFirstLosersStage = stageNr == numberOfStages - 1;

                    //Only the first stage of the losers bracket will be connected to the losing seeds
                    KoStage newStage = await createBracketStage(context, prevStage, stageNr, !isFirstLosersStage);

                    //Change name of the group and adjust group stage
                    newStage.StageGroup.GroupName = $"Losers - {newStage.StageGroup.GroupName}";
                    newStage.StageGroup.GroupOrderValue++;
                    context.Groups.Update(newStage.StageGroup);

                    //Adjust match stages
                    newStage.StageMatches.ForEach(m => m.MatchStage++);
                    context.UpdateRange(newStage.StageMatches);

                    prevStage = newStage;
                }

                //Add final final of both brackets by creating a final stage
                KoStage finalFinalStage = await createBracketStage(context, new KoStage() { Round = r, NumberOfStages = 1 }, 1, null);

                //update all values of this final final
                finalFinalStage.StageGroup.GroupOrderValue = 1;
                finalFinalStage.StageGroup.GroupName = "Turnierfinale";
                Match finalFinal = finalFinalStage.StageMatches.First();
                finalFinal.MatchStage = 1;

                //Link the matches
                winnersFinalStage.StageMatches.First().WinnerFollowUpMatch = finalFinal;
                prevStage.StageMatches.First().WinnerFollowUpMatch = finalFinal;

                //finalFinal.Update();
            }
            
            //save changes to database
            await context.SaveChangesAsync();

            return true;
        }

        private static async Task<KoStage> createBracketStage(ChemodartsContext context, KoStage previousStage, int stageNr, bool? connectMatchWinners)
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
                GroupOrderValue = getStageOrderFromStage(newStage),
                GroupName = $"{getGroupName(numberOfPlayersInStage)}",
            };
            context.Groups.Add(newStage.StageGroup);
            await context.SaveChangesAsync();

            //Make Matches
            int numberOfMatchesInStage = numberOfPlayersInStage / 2;
            int matchNr = 0;
            for (int matchStageNr = 0; matchStageNr < numberOfMatchesInStage; matchStageNr++)
            {
                Match m = new Match()
                {
                    GroupId = newStage.StageGroup.GroupId,
                    MatchStage = getStageOrderFromStage(newStage), 
                    MatchOrderValue = ++matchNr,
                };
                newStage.StageMatches.Add(m);
            }
            context.Matches.AddRange(newStage.StageMatches);
            await context.SaveChangesAsync(); //save to get matchIds

            //register as follow up if not base stage
            if (connectMatchWinners.HasValue && previousStage.StageMatches is object && previousStage.StageMatches.Count == 2 * newStage.StageMatches.Count)
            {
                for (int i = 0; i < newStage.StageMatches.Count; i++)
                {
                    if (connectMatchWinners.Value)
                    {
                        previousStage.StageMatches[2 * i].WinnerFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i + 1].WinnerFollowUpMatch = newStage.StageMatches[i];
                    }
                    else
                    {
                        previousStage.StageMatches[2 * i].LoserFollowUpMatch = newStage.StageMatches[i];
                        previousStage.StageMatches[2 * i + 1].LoserFollowUpMatch = newStage.StageMatches[i];
                    }
                }
                await context.SaveChangesAsync();
            }

            return newStage;
        } 

        private static int getStageOrderFromStage(KoStage s)
        {
            switch(s.Round.Modus)
            {
                case RoundModus.SingleKo:
                    return s.StageNr;
                case RoundModus.DoubleKo:
                    return 2 * s.StageNr;
                default:
                    return -1;
            }
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

            return r.Groups.MaxBy(g => g.GroupOrderValue)?.Matches.ToList();
        }

        public static Group? GetFirstRoundGroup(Round r)
        {
            return r.Groups?.MaxBy(g => g.GroupOrderValue);
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
            List<Match>? frm = GetFirstRoundMatches(r);
            if (frm is null) return;

            foreach(Match m in frm)
            {
                m.UpdateUpwards();
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
