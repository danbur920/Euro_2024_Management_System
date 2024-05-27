using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euro_2024_Management_System.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BetController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BetController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET:

        [HttpGet]
        public async Task<IActionResult> GetBets()
        {
            var bets = await _context.Bets.ToArrayAsync();
            return Ok(bets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBetById(int id)
        {
            var bet = await _context.Bets.FindAsync(id);
            return Ok(bet);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetBetsByUserId(string id)
        {
            var bets = await _context.Bets.
                Where(x => x.UserId == id).
                ToArrayAsync();
            return Ok(bets);
        }

        // POST:

        [HttpPost]
        public async Task<IActionResult> PostBet(Bet bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bets.Add(bet);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBetById), new { id = bet.Id }, bet);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(int id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest("ID zakładu w URL i w treści nie są zgodne.");
            }

            _context.Entry(bet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool BetExists(int id)
        {
            return _context.Bets.Any(e => e.Id == id);
        }
    }
}
