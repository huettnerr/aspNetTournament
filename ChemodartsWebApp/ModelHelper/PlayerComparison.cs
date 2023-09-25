using ChemodartsWebApp.Models;

namespace ChemodartsWebApp.ModelHelper
{
    public class PlayerComparison
    {
        public List<Match>? MatchesBetween { get; private set; }
        public Seed? StatsSeed { get; private set; }

        public PlayerComparison(Player? p1, Player? p2) 
        {
            if (p1 == null || p2 == null) return;

            MatchesBetween = p1.Matches?.Where(m => m.IsMatchOfPlayers(p1, p2))?.ToList();
            if(MatchesBetween?.Count == 0) return;    

            StatsSeed = new Seed();
            StatsSeed.Statistics = new SeedStatistics(ScoreType.LegsOnly);
            MatchesBetween?.ForEach(m => m.UpdateSeedStat((m.Seed1.Player.Equals(p1) ? m.Seed1 : m.Seed2), StatsSeed.Statistics));
        }
    }
}
