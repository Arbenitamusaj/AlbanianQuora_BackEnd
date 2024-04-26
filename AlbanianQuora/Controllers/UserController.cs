using AlbanianQuora.Data;
using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Web.Http.Cors;
using Microsoft.AspNetCore.Authorization;
using AlbanianQuora.DTO;
namespace AlbanianQuora.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "", methods: "")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _context.Users.ToListAsync();
            return Ok(user);
        }*/

        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> GetUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User ID is invalid");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }



        [HttpPut]
        [Authorize]
        public async Task<IActionResult> PutUser([FromBody] UserPutDTO userDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User ID is invalid");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Password = userDto.Password;


            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) { return NotFound(); }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}