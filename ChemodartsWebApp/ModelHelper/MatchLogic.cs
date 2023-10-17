using ChemodartsWebApp.Data;
using ChemodartsWebApp.Models;
using static ChemodartsWebApp.Models.Match;

namespace ChemodartsWebApp.ModelHelper
{
    public class MatchLogic
    {
        private readonly ChemodartsContext _context;
        public MatchLogic(ChemodartsContext context) 
        { 
            _context = context;
        }

        public bool SetNewStatus(Match m, MatchStatus newStatus)
        {
            bool statusChanged = !m.Status.Equals(newStatus);
            switch (newStatus)
            {
                case MatchStatus.Active:
                    //both seeds are null, dummy or bye. Dont start match
                    if ((m.Seed1 is null || m.Seed1.IsDummy || m.Seed1.IsByeSeed()) && (m.Seed2 is null || m.Seed2.IsDummy || m.Seed2.IsByeSeed())) return false;
                    break;
                case MatchStatus.Created:
                    if(m.Score is object) _context.Scores.Remove(m.Score);
                    m.Score = null;
                    break;
                case MatchStatus.Finished:
                    m.Venue = null;
                    break;
            }

            m.Status = newStatus;

            Update(ref m, statusChanged);

            _context.Matches.Update(m);
            _context.SaveChanges();

            return true;
        }

        public void Update(ref Match m, bool statusChanged)
        {
            switch (m.Group.Round.Modus)
            {
                case RoundModus.RoundRobin:
                    if (m.HandleWinnerLoserSeedOfMatch() || statusChanged)
                    {
                        Ranking.UpdateGroupRanking(m.Group);
                    }
                    break;
                case RoundModus.SingleKo:
                case RoundModus.DoubleKo:
                    CheckWinnerAndHandleFollowUpMatches(ref m, statusChanged);
                    break;
            }
        }

        /// <summary>
        /// Checks if the Match has a winner and if so, it propagates the update to the following matches
        /// </summary>
        /// <param name="statusChanged">If the statusChanged is true the updated seeds will be propagated either way</param>
        public void CheckWinnerAndHandleFollowUpMatches(ref Match m, bool statusChanged)
        {
            bool hasMatchWinner = m.HandleWinnerLoserSeedOfMatch();

            if (hasMatchWinner || statusChanged)
            {
                //Match has winner. Handle Follow-Ups
                Match? wfu = m.WinnerFollowUpMatch;
                if (wfu is object)
                {
                    if (m.WinnerFollowUpSeedNr == 1)
                    {
                        if (hasMatchWinner && (wfu.Seed1?.IsDummy ?? false)) _context.Seeds.Remove(wfu.Seed1);
                        wfu.Seed1 = hasMatchWinner ? m.WinnerSeed : null;
                    }
                    else if (m.WinnerFollowUpSeedNr == 2)
                    {
                        if (hasMatchWinner && (wfu.Seed2?.IsDummy ?? false)) _context.Seeds.Remove(wfu.Seed2);
                        wfu.Seed2 = hasMatchWinner ? m.WinnerSeed : null;
                    }

                    CheckWinnerAndHandleFollowUpMatches(ref wfu, statusChanged);
                    //_context.Matches.Update(wfu);
                }

                Match? lfu = m.LoserFollowUpMatch;
                if (lfu is object)
                {
                    if (m.LoserFollowUpSeedNr == 1)
                    {
                        if (hasMatchWinner && (lfu.Seed1?.IsDummy ?? false)) _context.Seeds.Remove(lfu.Seed1);
                        lfu.Seed1 = hasMatchWinner ? m.LoserSeed : null;
                    }
                    else if (m.LoserFollowUpSeedNr == 2)
                    {
                        if (hasMatchWinner && (lfu.Seed2?.IsDummy ?? false)) _context.Seeds.Remove(lfu.Seed2);
                        lfu.Seed2 = hasMatchWinner ? m.LoserSeed : null;
                    }

                    CheckWinnerAndHandleFollowUpMatches(ref lfu, statusChanged);
                    //_context.Matches.Update(lfu);
                }
            }
        }
    }
}
