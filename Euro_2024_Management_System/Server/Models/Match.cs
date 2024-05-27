using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euro_2024_Management_System.Server.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("HomeTeam")]
        public int HomeTeamId { get; set; }
        [ForeignKey("AwayTeam")]
        public int AwayTeamId { get; set; }
        public int Round { get; set; }
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public int? GoalsHome { get; set; }
        public int? GoalsAway { get; set; }
        public int? GoalsCount { get; set; }
        public bool IsFinished {  get; set; }
        public int? Result { get; set; }
        public virtual Team? HomeTeam { get; set; }
        public virtual Team? AwayTeam { get; set; }
        public virtual ICollection<Bet>? Bets { get; set; }

        public Match()
        {
            IsFinished = false;
        }
    }
}
