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
        [Display(Name = "Folgerunde:")] public int? FollowUpRoundId { get; set; }


        [ScaffoldColumn(false)]
        public Tournament? T { get; set; }

        public RoundFactory() { } //Needed for POST
        public RoundFactory(string action, Tournament t, Round? r) : base(action)
        {
            T = t;
            if (r is object)
            {
                Name = r.RoundName;
                RoundModus = r.Modus;
                Scoring = r.Scoring;
                FollowUpRoundId = r.FollowUpRoundId;
                //IsStarted = r.IsRoundStarted;
                //IsFinished = r.IsRoundFinished;
            }
            FollowUpRoundId = 0;
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
            r.FollowUpRoundId = FollowUpRoundId;
            //r.IsRoundStarted = IsStarted;
            //r.IsRoundFinished = IsFinished;
        }
    }
}
