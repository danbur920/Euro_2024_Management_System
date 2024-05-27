using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euro_2024_Management_System.Server.Models
{
    public class Bet
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public DateTime? BetDate { get; set; }
        public int? MatchBet { get; set; }
        public int? GoalsHome { get; set; }
        public int? GoalsAway { get; set; }
        public int? GoalsCount { get; set; }
        public int? PointsScored { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual Match? Match { get; set; }
    }
}
