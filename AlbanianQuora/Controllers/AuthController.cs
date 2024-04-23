using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using AlbanianQuora.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using AlbanianQuora.Services;
using System.Security.Claims;
using System.Web.Http.Cors;
using BCrypt.Net;
using AlbanianQuora.DTO;
namespace AlbanianQuora.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [ApiController]
    [Route("[controller]")]
    public class Authcontroller(UserDbContext context) : ControllerBase
    {
        private readonly UserDbContext _context = context;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userDto)
        {
            if (_context.Users.Any(u => u.Email == userDto.Email))
            {
                return BadRequest("User already exists.");
            }

            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return Unauthorized("Email or password did not match");
            }

            var token = TokenService.GenerateToken(user.UserId);
            return Ok(new { token = token });
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

            if (!Guid.TryParse(idClaim.Value, out Guid id))
            {
                return Unauthorized("Invalid user ID claim");
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound("user not found");
            }

            return Ok(new { Message = "User registered successfully", UUID = user.UserId });
        }
    }
}

