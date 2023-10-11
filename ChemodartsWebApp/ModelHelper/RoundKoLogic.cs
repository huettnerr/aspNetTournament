using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.ModelHelper
{
    public static class RoundKoLogic
    {
        public static async Task<bool> CreateSystemSingleKO(ChemodartsContext context, Round r, int numberOfPlayers, bool createSeeds)
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

                //Make seeds if neccesary
                if (createSeeds && stageNr == numberOfStages)
                {
                    List<Seed> seeds = new List<Seed>();
                    for (int seedNr = 0; seedNr < numberOfPlayers; seedNr++)
                    {
                        Seed s = new Seed()
                        {
                            SeedName = $"KO-Seed #{seedNr}",
                            GroupId = g.GroupId,
                        };
                        seeds.Add(s);
                    }

                    context.Seeds.AddRange(seeds);
                    await context.SaveChangesAsync();

                    matches = RandomizeSeedsForMatches(matches, seeds);
                    context.Matches.UpdateRange(matches);
                    await context.SaveChangesAsync();
                }
            }

            //After creating this list should contain only the final. Updating it will recursivly update all matches
            prevStageMatches?.FirstOrDefault()?.UpdateSeedsFromAcestors();
            await context.SaveChangesAsync();

            return true;
        }

        public static List<Match> RandomizeSeedsForMatches(List<Match> matches, List<Seed> seeds)
        {
            Seed randomSeed;
            Random random = new Random();

            //Randomize 1st seed
            foreach (Match m in matches)
            {
                //Set seed 1
                randomSeed = seeds.ElementAt(random.Next(seeds.Count));
                m.Seed1Id = randomSeed.SeedId;
                seeds.Remove(randomSeed);
            }

            //Fill with bye's if necessary
            Seed byeSeed = new Seed() { SeedName = "Bye" };
            while (seeds.Count < matches.Count) seeds.Add(byeSeed);

            //Randomize 2nd seed
            foreach (Match m in matches)
            {
                //set seed 2
                randomSeed = seeds.ElementAt(random.Next(seeds.Count));
                seeds.Remove(randomSeed);

                //Handle bye seeds
                if (!randomSeed.Equals(byeSeed))
                {
                    m.Seed2Id = randomSeed.SeedId;
                }
                else
                {
                    m.SetNewStatus(Match.MatchStatus.Finished);
                }
            }

            return matches;
        }

        public static async Task<bool> CreateSystem(ChemodartsContext context, OldGroupFactoryKO factory, Round r, ModelStateDictionary? modelState)
        {
            if (r is null || r.Modus != RoundModus.SingleKo) return false;

            context.Groups.RemoveRange(r.Groups);
            await context.SaveChangesAsync();

            factory.R = r;
            factory.NumberOfRounds--; // Fühlt sich natürlicher an das Finale mitzuzählen

            List<Group> groups = new List<Group>();
            for (int roundNr = 0; roundNr <= factory.NumberOfRounds; roundNr++)
            {
                //Make Groups
                int playersInRound = 2 * getPlayersPerStage(factory.NumberOfRounds - roundNr);
                Group g = new Group()
                {
                    GroupName = getGroupName(playersInRound),
                    RoundId = factory.R.RoundId,
                };
                g.Matches = new List<Match>();
                groups.Add(g);
                context.Groups.Add(g);
                context.SaveChanges();

                //Make Seeds
                List<Seed> seeds = new List<Seed>();
                for (int iPlayer = 0; iPlayer < playersInRound; iPlayer++)
                {
                    Seed s = new Seed()
                    {
                        SeedName = "Please Run Script",
                        GroupId = g.GroupId,
                    };

                    if (roundNr > 1)
                    {
                        //Link Ancestors
                        s.AncestorMatch = groups.ElementAt(roundNr - 1).Matches.ElementAt(iPlayer);
                    }
                    else
                    {
                        //First round
                        s.SeedNr = iPlayer;
                    }

                    seeds.Add(s);
                }
                context.Seeds.AddRange(seeds);
                context.SaveChanges();

                //Make Matches
                List<Match> matches = new List<Match>();
                for (var iMatch = 0; iMatch < getPlayersPerStage(factory.NumberOfRounds - roundNr); iMatch++)
                {
                    Match m = new Match()
                    {
                        Seed1Id = seeds.ElementAt(2 * iMatch).SeedId,
                        Seed2Id = seeds.ElementAt(2 * iMatch + 1).SeedId,
                        MatchOrderValue = iMatch,
                        GroupId = g.GroupId,
                    };
                    g.Matches.Add(m);
                    matches.Add(m);
                }
                context.Matches.AddRange(matches);
                context.SaveChanges();
            }

            return true;
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
                    m.UpdateSeedsFromAcestors();
                }
            }

            context.SaveChanges();
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
