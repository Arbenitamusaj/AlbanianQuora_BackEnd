using AlbanianQuora.Data;
using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlbanianQuora.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserDbContext _context;

        public UserController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await _context.Users.ToListAsync();
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (!UserExists(id))
                {
                    return NotFound(ex.Message);
                }
                else
                {
                    throw;
                }
            }
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