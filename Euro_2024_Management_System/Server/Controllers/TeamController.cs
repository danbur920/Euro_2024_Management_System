using Euro_2024_Management_System.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Euro_2024_Management_System.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET:

        [HttpGet]
        public async Task<IActionResult> GetTeams()
        {
            var teams = await _context.Teams.ToArrayAsync();
            return Ok(teams);
        }

        [HttpGet("{id}")]
        public async Task <IActionResult> GetTeamById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            return Ok(team);
        }

        [HttpGet("group/{group}")]
        public async Task<IActionResult> GetTeamsByGroup(string group)
        {
            var teams = await _context.Teams.Where(x => x.Group == group).ToArrayAsync();
            return Ok(teams);
        }
    }
}
