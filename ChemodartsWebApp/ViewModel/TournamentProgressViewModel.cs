using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;

namespace ChemodartsWebApp.ViewModel
{
    public class TournamentProgressViewModel : RoundViewModel
    {
        public MapTournamentProgression.TournamentProgressionType ProgressionType { get; set; }
        public int ByeCount { get; set; }
        public int AdvanceCount { get; set; }
    }
}
