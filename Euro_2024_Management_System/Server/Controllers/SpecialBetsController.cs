using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Euro_2024_Management_System.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecialBetsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public SpecialBetsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        // GET:

        [HttpGet("specialBets")]
        public async Task<IActionResult> GetSpecialBets()
        {
            var bets = await _context.SpecialBets.ToArrayAsync();
            return Ok(bets);
        }

        [HttpGet("userSpecialBets")]
        public async Task<IActionResult> GetUserSpecialBets()
        {
            var bets = await _context.UserSpecialBets.ToArrayAsync();
            return Ok(bets);
        }

        [HttpGet("userSpecialBetsByLoggedUser")]
        public async Task<IActionResult> GetUserSpecialBetsByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bets = await _context.UserSpecialBets
                .Where(x => x.ApplicationUserId == userId)
                .ToArrayAsync();

            return Ok(bets);
        }

        [HttpPut("userSpecialBets/{id}")]
        public async Task<IActionResult> PutUserSpecialBetByUserId(int id, UserSpecialBet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest("ID zakładu w URL i w treści nie są zgodne.");
            }

            bet.IsApproved = !bet.IsApproved;
            _context.Entry(bet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return NoContent();
        }
    }
}
