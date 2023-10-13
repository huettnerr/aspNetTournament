using ChemodartsWebApp.Models;
using static ChemodartsWebApp.Models.Match;

namespace ChemodartsWebApp.ModelHelper
{
    public static class Ranking
    {
        public static Seed? GetWinnerSeed(Match m)
        {
            if (m.Status != MatchStatus.Finished) return null;

            if (m.Seed1 is object && m.Seed2 is null) return m.Seed1;
            if (m.Seed1 is null && m.Seed2 is object) return m.Seed2;

            if (m.Seed1 is null && m.Seed2 is null) return null;
            if (m.Score == null) return null;


            if (m.Group.Round.Scoring == ScoreType.LegsOnly)
            {
                //Check for Legs
                if (m.Score.P1Legs > m.Score.P2Legs)
                {
                    //Seed 1 won
                    return m.Seed1;
                }
                else if (m.Score.P1Legs < m.Score.P2Legs)
                {
                    //Seed 2 won
                    return m.Seed2;
                }
                return null;
            }
            else
            {
                //Check for Sets
                if (m.Score.P1Sets > m.Score.P2Sets)
                {
                    //Player 1 won
                    return m.Seed1;
                }
                else if (m.Score.P1Sets < m.Score.P2Sets)
                {
                    //Player 2 won
                    return m.Seed2;
                }
                return null;
            }
        }

        public static void CalculateSeedStatistics(Seed s, Group? g = null)
        {
            //Create Stats if necessary
            if (s.SeedStatistics is null)
            {
                s.SeedStatistics = new SeedStatistics() { SeedId = s.SeedId };
            }

            s.SeedStatistics.ResetStatistic();

            s.SeedStatistics.CalculateCombinedMatchStatistic(s.Matches.Where(m => g?.Equals(m.Group) ?? true).ToList(), s.Player);
        }

        public static void CalculateCombinedMatchStatistic(this SeedStatistics stats, List<Match>? matches, Player p)
        {
            if(matches is null) return;

            //Iterate over Matches
            foreach (Match m in matches)
            {
                //only calculate for finished matches with valid score
                if (m.Status != MatchStatus.Finished) continue;
                if (m.Score == null) continue;

                //Update Set/Leg Stat
                if (m.Seed1.Player?.Equals(p) ?? false)
                {
                    //Won
                    stats.SetsWon += m.Score.P1Sets;
                    stats.LegsWon += m.Score.P1Legs;

                    //Lost
                    stats.SetsLost += m.Score.P2Sets;
                    stats.LegsLost += m.Score.P2Legs;
                }
                else if (m.Seed2.Player?.Equals(p) ?? false)
                {
                    //Won
                    stats.SetsWon += m.Score.P2Sets;
                    stats.LegsWon += m.Score.P2Legs;

                    //Lost
                    stats.SetsLost += m.Score.P1Sets;
                    stats.LegsLost += m.Score.P1Legs;
                }
                else continue;

                //Update Matches Stat
                stats.Matches++;
                if (m.WinnerSeed.Player.Equals(p))
                {
                    stats.MatchesWon++;
                }
                else if (m.WinnerSeed is null)
                {
                    stats.MatchesTied++;
                }
                else
                {
                    stats.MatchesLost++;
                }
            }

            return;
        }

        public static void UpdateGroupRanking(Group g)
        {
            //Update Statistics
            g.Seeds.ToList().ForEach(s => CalculateSeedStatistics(s, g));

            //Order the seeds
            List<Seed> orderedSeeds = g.Seeds
                .OrderByDescending(s => s.SeedStatistics.MatchesWon)
                .ThenByDescending(s => s.SeedStatistics.PointsDiff)
                .ThenByDescending(s => s.SeedStatistics.PointsFor).ToList();

            //Update ranking
            int rank = 1;
            orderedSeeds.ForEach(s => s.SeedRank = rank++);

            //Direct Comparison
            for (int i = 0; i < orderedSeeds.Count - 1; i++)
            {
                Seed s1 = orderedSeeds[i];
                Seed s2 = orderedSeeds[i + 1];

                //Check if some adjecent seed statistics are totally identical
                if (s1.SeedStatistics.MatchesWon == s2.SeedStatistics.MatchesWon &&
                    s1.SeedStatistics.PointsDiff == s2.SeedStatistics.PointsDiff &&
                    s1.SeedStatistics.PointsFor == s2.SeedStatistics.PointsFor)
                {
                    List<Match>? directMatches = s1.Group.Matches.Where(m => m.IsMatchOfSeeds(s1, s2)).ToList();
                    int winsOfS1 = directMatches.Count(m => s1.Equals(m.WinnerSeed));
                    int winsOfS2 = directMatches.Count(m => s2.Equals(m.WinnerSeed));

                    // seed 2 wins direct comparison
                    if (winsOfS2 > winsOfS1)
                    {
                        // seed 2 wins direct comparison -> swap ranks
                        int rankS1 = s1.SeedRank;
                        s1.SeedRank = s2.SeedRank;
                        s2.SeedRank = rankS1;
                    }
                    else if (winsOfS1 == winsOfS2)
                    {
                        s2.SeedRank = s1.SeedRank;
                    }
                }
            }

            //Update ranks to original list
            foreach (Seed s in g.Seeds)
            {
                s.SeedRank = orderedSeeds.Find(os => os.Equals(s))?.SeedRank ?? 0;
            }
        }
    }
}
