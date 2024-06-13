using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Euro_2024_Management_System.Server.Models
{
    public class UserSpecialBet
    {
        [Key]
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? SpecialBetId { get; set; }
        public int? Points { get; set; }
        public bool IsSettled { get; set; }
        public bool IsApproved { get; set; }
        public string? UserBet { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual SpecialBet? SpecialBet { get; set; }
    }
}
