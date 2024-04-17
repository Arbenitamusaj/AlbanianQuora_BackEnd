using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using AlbanianQuora.Data;
using System.Security.Cryptography;
using System.Text;
using AlbanianQuora.Services;
using System.Security.Claims;
namespace AlbanianQuora.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Authcontroller(UserDbContext context) : ControllerBase
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


        [HttpPost("/login")]

        public IActionResult Login([FromBody] UserLogin request)
        {
            var user = context.Users.Where(user => user.Email == request.Email)
                                      .Where(user => user.Password == request.Password)
                                      .FirstOrDefault();
            if (user == null)
            {
                return Unauthorized("Email or password did not match");
            }
            var token = TokenService.GenerateToken(user.UserId);

            return Ok(new Dictionary<string, string>() { { "token", token } });
        }

        [HttpGet("me")]
        public IActionResult GetById([FromQuery] string token)
        {
            var principal = TokenService.VerifyToken(token);
            if (principal == null)
            {
                return Unauthorized("Invalid token.");
            }

            var idClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                return Unauthorized("Token does not contain required claim.");
            }

            if (!int.TryParse(idClaim.Value, out var id))
            {
                return Unauthorized("Invalid user ID claim");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("user not found");
            }

            return Ok(user); // Assuming you want to return user details
        }
    }
}

