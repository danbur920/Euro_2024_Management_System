using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Euro_2024_Management_System.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public int? Points { get; set; }
        public int? CorrectResults { get; set; } // ilość poprawnych wyników (np 2:1)
        public int? CorrectBets { get; set; } // ilość poprawnych typów (np. 0 - remis)
        public virtual ICollection<Bet>? Bets { get; set; }
    }
}
