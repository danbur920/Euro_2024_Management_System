using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euro_2024_Management_System.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public MatchController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET:

        [HttpGet]
        public async Task<IActionResult> GetMatches()
        {
            var matches = await _context.Matches.ToArrayAsync();
            return Ok(matches);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMatchById(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            return Ok(match);
        }

        [HttpGet("round/{round}")]
        public async Task<IActionResult> GetMatchesByRound(int round)
        {
            var matches = await _context.Matches.
                Where(x => x.Round == round).
                ToArrayAsync();
            return Ok(matches);
        }

        // POST:

        [HttpPost]
        public async Task<IActionResult> PostMatch(Match match)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatchById), new { id = match.Id }, match);
        }
    }
}
