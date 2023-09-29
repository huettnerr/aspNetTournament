using ChemodartsWebApp.Data;
using ChemodartsWebApp.Data.Factory;
using ChemodartsWebApp.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChemodartsWebApp.ModelHelper
{
    public static class RoundKoLogic
    {
        public static async Task<bool> CreateSystem(ChemodartsContext context, GroupFactoryKO factory, Round r, ModelStateDictionary? modelState)
        {
            if (r is null || r.Modus != RoundModus.SingleKo) return false;

            context.Groups.RemoveRange(r.Groups);
            await context.SaveChangesAsync();

            factory.R = r;
            factory.NumberOfRounds--; // Fühlt sich natürlicher an das Finale mitzuzählen

            List<Group> groups = new List<Group>();
            for (int roundNr = 0; roundNr <= factory.NumberOfRounds; roundNr++)
            {

                int playersInRound = 2 * getMatchesPerRound(factory.NumberOfRounds - roundNr);
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
                for (var iMatch = 0; iMatch < getMatchesPerRound(factory.NumberOfRounds - roundNr); iMatch++)
                {
                    Match m = new Match()
                    {
                        Seed1Id = seeds.ElementAt(2 * iMatch).SeedId,
                        Seed2Id = seeds.ElementAt(2 * iMatch + 1).SeedId,
                        MatchOrderValue = iMatch,
                        GroupId = g.GroupId,
                        Status = Match.MatchStatus.Created,
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
            for (int i = 1; i < r.Groups.Count; i++)
            {
                int matchNr = 0;
                foreach (Match m in r.Groups.ElementAt(i).Matches)
                {
                    Match mForS1 = r.Groups.ElementAt(i - 1).Matches.ElementAt(2 * matchNr);
                    if (mForS1.WinnerSeed is object) m.Seed1Id = mForS1.WinnerSeed.SeedId;
                    else m.Seed1.SeedName = $"{mForS1.Seed1.Player?.PlayerDartname ?? mForS1.Seed1.SeedName} | {mForS1.Seed2.Player?.PlayerDartname ?? mForS1.Seed2.SeedName}";

                    Match mForS2 = r.Groups.ElementAt(i - 1).Matches.ElementAt(2 * matchNr + 1);
                    if (mForS2.WinnerSeed is object) m.Seed2Id = mForS2.WinnerSeed.SeedId;
                    else m.Seed2.SeedName = $"{mForS2.Seed1.Player?.PlayerDartname ?? mForS2.Seed1.SeedName} | {mForS2.Seed2.Player?.PlayerDartname ?? mForS2.Seed2.SeedName}";

                    matchNr++;
                }
            }

            context.SaveChanges();
        }

        private static int getMatchesPerRound(int depth)
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
