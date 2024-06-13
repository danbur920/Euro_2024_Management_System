using Euro_2024_Management_System.Server.Data;
using Euro_2024_Management_System.Server.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("changeState/{id}")]
        public async Task<IActionResult> ChangeStateBet(int id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest("ID zakładu w URL i w treści nie są zgodne.");
            }

            bet.IsApproved = false;
            bet.BetDate = null;
            bet.GoalsHome = null;
            bet.GoalsAway = null;
            bet.GoalsCount = null;
            bet.MatchBet = null;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutBet(int id, Bet bet)
        {
            if (id != bet.Id)
            {
                return BadRequest("ID zakładu w URL i w treści nie są zgodne.");
            }

            bet.IsApproved = true;
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

        [HttpPost("summarize")]
        public async Task<IActionResult> SummarizeBets()
        {
            Console.WriteLine($"{User.Identity.Name}");

            var users = await _context.Users.ToListAsync();
            var bets = await _context.Bets.ToListAsync();
            var matches = await _context.Matches.ToListAsync();

            foreach (var user in users)
            {
                var userBets = bets.Where(x => x.UserId == user.Id && x.MatchBet != null).ToArray();
                if (user.Points == null)
                    user.Points = 0;

                if(user.CorrectResults == null)
                    user.CorrectResults = 0;

                if (user.CorrectBets == null)
                    user.CorrectBets = 0;

                foreach (var bet in userBets)
                {
                    var match = matches.FirstOrDefault(x => x.Id == bet.MatchId);
                    if (match.IsFinished && bet.IsSettled == false)
                    {
                        bet.PointsScored = 0;

                        // Dokładny wynik:
                        if (bet.GoalsHome == match.GoalsHome && bet.GoalsAway == match.GoalsAway)
                        {
                            user.Points += 5;
                            bet.PointsScored += 5;
                            user.CorrectResults++;
                        }

                        // Dokładna ilość goli:
                        var goalsCount = match.GoalsHome + match.GoalsAway;
                        if (bet.GoalsCount == goalsCount)
                        {
                            user.Points += 2;
                            bet.PointsScored += 2;
                        }

                        // Remis (0):
                        if (bet.MatchBet == 0 && match.Result == 0)
                        {
                            user.Points += 3;
                            bet.PointsScored += 3;
                            user.CorrectBets++;
                        }

                        // Wygrana gości (1):
                        if (bet.MatchBet == 1 && match.Result == 1)
                        {
                            user.Points += 2;
                            bet.PointsScored += 2;
                            user.CorrectBets++;
                        }

                        // Wygrana gospodarzy (2):
                        if (bet.MatchBet == 2 && match.Result == 2)
                        {
                            user.Points += 2;
                            bet.PointsScored += 2;
                            user.CorrectBets++;
                        }

                        // Podpórka 10:
                        if (bet.MatchBet == 10)
                        {
                            if (!(match.GoalsAway > match.GoalsHome))
                            {
                                user.Points += 1;
                                bet.PointsScored += 1;
                                user.CorrectBets++;
                            }
                        }

                        // Podpórka 02: (BABOL) wykonuje się dla typu równego: 2 EDIT: Fixed 02 ---> 20
                        if (bet.MatchBet == 20)
                        {
                            if (!(match.GoalsAway < match.GoalsHome))
                            {
                                user.Points += 1;
                                bet.PointsScored += 1;
                                user.CorrectBets++;
                            }
                        }

                        // Podpórka 12:
                        if (bet.MatchBet == 12 && match.Result != 0)
                        {
                            user.Points += 1;
                            bet.PointsScored += 1;
                            user.CorrectBets++;
                        }

                        bet.IsSettled = true;
                    }
                    _context.Bets.Update(bet);
                }
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Zakończono aktualizację" });
        }
    }
}
