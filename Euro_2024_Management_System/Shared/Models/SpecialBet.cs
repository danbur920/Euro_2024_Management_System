using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Euro_2024_Management_System.Shared.Models
{
    public class SpecialBet
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<UserSpecialBet>? UserSpecialBets { get; set; }
    }
}
