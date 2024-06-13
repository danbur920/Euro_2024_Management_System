using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Euro_2024_Management_System.Server.Models
{
    public class SpecialBet
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<UserSpecialBet>? UserSpecialBets { get; set; }
    }
}
