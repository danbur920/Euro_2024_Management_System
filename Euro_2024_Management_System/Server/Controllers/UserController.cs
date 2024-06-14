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
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userManager.Users.OrderByDescending(x=>x.Points).ToArrayAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateUser(ApplicationUser user)
        //{
        //    var result = await _userManager.CreateAsync(user, "DefaultPassword123!");
        //    if (result.Succeeded)
        //    {
        //        return Ok(user);
        //    }
        //    return BadRequest(result.Errors);
        //}

        [HttpGet("is-admin")]
        public async Task<IActionResult> IsAdmin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // This way because Roles not working :(
            if (userId == "09ccc57d-efe2-42c8-9365-bdbc109980c8")
            {
                return Ok(true);
            }

            return Ok(false);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(ApplicationUser updatedUser)
        {
            var existingUser = await _userManager.FindByIdAsync(updatedUser.Id);

            if (existingUser == null)
            {
                return NotFound("Użytkownik nie istnieje.");
            }

            if (await _roleManager.RoleExistsAsync("Gracz"))
            {
                var roles = await _userManager.GetRolesAsync(existingUser);
                await _userManager.RemoveFromRolesAsync(existingUser, roles.ToArray());

                await _userManager.AddToRoleAsync(existingUser, "Gracz");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Rola 'Gracz' nie istnieje.");
            }

            // Aktualizujemy pozostałe właściwości użytkownika
            existingUser.FirstName = "";
            existingUser.LastName = "";
            existingUser.Age = 5;

            var updateResult = await _userManager.UpdateAsync(existingUser);

            if (!updateResult.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Nie udało się zaktualizować danych użytkownika.");
            }

            return Ok(existingUser);
        }
    }
}
