using ChemodartsWebApp.Models;
using System.ComponentModel.DataAnnotations;

namespace ChemodartsWebApp.Data.Factory
{
    public class RoundFactory : FactoryBase<Round>
    {
        public override string Controller { get; } = "Round";

        public string Name { get; set; }
        public RoundModus RoundModus { get; set; }
        public ScoreType Scoring { get; set; }
        //public bool IsStarted { get; set; }
        //public bool IsFinished { get; set; }


        [ScaffoldColumn(false)]
        public Tournament? T { get; set; }

        public RoundFactory() { } //Needed for POST
        public RoundFactory(string action, Round r) : base(action)
        {
            if (r is object)
            {
                Name = r.RoundName;
                RoundModus = r.Modus;
                Scoring = r.Scoring;
                //IsStarted = r.IsRoundStarted;
                //IsFinished = r.IsRoundFinished;
            }
        }

        public override Round? Create()
        {
            if (T is null) return null;

            Round r = new Round();
            Update(ref r);
            r.TournamentId = T.TournamentId;

            return r;
        }

        public override void Update(ref Round r)
        {
            r.RoundName = Name;
            r.Modus = RoundModus;
            r.Scoring = Scoring.Equals(ScoreType.Default) ? ScoreType.LegsOnly : Scoring;
            //r.IsRoundStarted = IsStarted;
            //r.IsRoundFinished = IsFinished;
        }
    }
}
