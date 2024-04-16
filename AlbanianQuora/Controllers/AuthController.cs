using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using AlbanianQuora.Data;
using System.Security.Cryptography;
using System.Text;
namespace AlbanianQuora.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authcontroller (UserDbContext context) : ControllerBase
    {
        private readonly UserDbContext _context = context;

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            // Check if user already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                return BadRequest("User already exists.");
            }

            // Hash the password
            using var hmac = new HMACSHA512();
            user.Password = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)));

            // Add user to the database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }
    }
}
