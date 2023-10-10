using ChemodartsWebApp.Models;
using ChemodartsWebApp.Data.Factory;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChemodartsWebApp.ViewModel
{
    public class TournamentProgressViewModel : RoundViewModel
    {
        public MapTournamentProgression? ProgressionSetting { get; set; }
    }
}
