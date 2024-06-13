using Microsoft.AspNetCore.Identity;

namespace Euro_2024_Management_System.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public int? Points {  get; set; }
        public int? CorrectResults { get; set; } // ilość poprawnych wyników (np 2:1)
        public int? CorrectBets { get; set; } // ilość poprawnych typów (np. 0 - remis)
        public virtual ICollection<Bet>? Bets { get; set; }
        public virtual ICollection<UserSpecialBet>? UserSpecialBets { get; set; }
    }
}
