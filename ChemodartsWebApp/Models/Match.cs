using System.ComponentModel.DataAnnotations;

namespace ChemodartsMySql.Models
{
    public class Match
    {
        public enum MatchStatus
        {
            Created,
            Active,
            Finished
        }

        public int MatchId { get; set; }
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
        public MatchStatus Status {get; set; }

        [DataType(DataType.Date)]
        public DateTime? TimeStarted { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TimeFinished { get; set; }
    }
}
