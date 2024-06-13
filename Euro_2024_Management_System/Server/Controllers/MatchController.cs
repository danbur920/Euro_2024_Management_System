using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, [FromBody] Match match)
        {
            if (id != match.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMatch = await _context.Matches.FindAsync(id);

            if (existingMatch == null)
                return NotFound();

            match.GoalsCount = match.GoalsHome + match.GoalsAway;
            match.IsFinished = true;

            if (match.GoalsHome > match.GoalsAway)
                match.Result = 1;

            if (match.GoalsHome < match.GoalsAway)
                match.Result = 2;

            if (match.GoalsHome == match.GoalsAway)
                match.Result = 0;

            // Dodaj statystyki drużynom:

            var existingStatsTeamHome = await _context.Teams.FindAsync(match.HomeTeamId);
            var existingStatsTeamAway = await _context.Teams.FindAsync(match.AwayTeamId);

            var teamHome = await _context.Teams.FindAsync(match.HomeTeamId);
            var teamAway = await _context.Teams.FindAsync(match.AwayTeamId);

            teamHome.GoalsScored += (int)match.GoalsHome;
            teamAway.GoalsScored += (int)match.GoalsAway;

            teamHome.GoalsConceded += (int)match.GoalsAway;
            teamAway.GoalsConceded += (int)match.GoalsHome;

            teamHome.GoalBalance = teamHome.GoalsScored - teamHome.GoalsConceded;
            teamAway.GoalBalance = teamAway.GoalsScored - teamAway.GoalsConceded;

            if(match.Result == 1)
            {
                teamHome.Wins++;
                teamAway.Losses++;

                teamHome.Points += 3;
            }
            else if(match.Result == 2)
            {
                teamHome.Losses++;
                teamAway.Wins++;

                teamAway.Points += 3;
            }
            else if(match.Result == 0)
            {
                teamHome.Draws++;
                teamAway.Draws++;

                teamHome.Points += 1;
                teamAway.Points += 1;
            }

            _context.Entry(existingStatsTeamHome).CurrentValues.SetValues(teamHome);
            _context.Entry(existingStatsTeamAway).CurrentValues.SetValues(teamAway);

            _context.Entry(existingMatch).CurrentValues.SetValues(match);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("state/{id}")]
        public async Task<IActionResult> UpdateState(int id, [FromBody] Match match)
        {
            if (id != match.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingMatch = await _context.Matches.FindAsync(id);

            var existingStatsTeamHome = await _context.Teams.FindAsync(match.HomeTeamId);
            var existingStatsTeamAway = await _context.Teams.FindAsync(match.AwayTeamId);

            if (existingMatch == null)
                return NotFound();

            // Cofnij statystyki drużynom:

            var teamHome = await _context.Teams.FindAsync(match.HomeTeamId);
            var teamAway = await _context.Teams.FindAsync(match.AwayTeamId);

            teamHome.GoalsScored -= (int)match.GoalsHome;
            teamAway.GoalsScored -= (int)match.GoalsAway;

            teamHome.GoalsConceded -= (int)match.GoalsAway;
            teamAway.GoalsConceded -= (int)match.GoalsHome;

            teamHome.GoalBalance = teamHome.GoalsScored - teamHome.GoalsConceded;
            teamAway.GoalBalance = teamAway.GoalsScored - teamAway.GoalsConceded;

            if (match.Result == 1)
            {
                teamHome.Wins--;
                teamAway.Losses--;

                teamHome.Points -= 3;
            }
            else if (match.Result == 2)
            {
                teamHome.Losses--;
                teamAway.Wins--;

                teamAway.Points -= 3;
            }
            else if (match.Result == 0)
            {
                teamHome.Draws--;
                teamAway.Draws--;

                teamHome.Points -= 1;
                teamAway.Points -= 1;
            }

            match.IsFinished = false;
            match.Result = null;
            match.GoalsHome = null;
            match.GoalsAway = null;
            match.GoalsCount = null;

            _context.Entry(existingStatsTeamHome).CurrentValues.SetValues(teamHome);
            _context.Entry(existingStatsTeamAway).CurrentValues.SetValues(teamAway);

            _context.Entry(existingMatch).CurrentValues.SetValues(match);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
