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
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly UserDbContext _context;

        public LikeController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("hasLiked/{questionId}")]
        [Authorize]
        public async Task<IActionResult> HasLiked(Guid questionId)
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

            var hasLiked = await _context.Likes.AnyAsync(l => l.UserId == userId && l.QuestionId == questionId);
            return Ok(hasLiked);
        }


        [HttpGet("count/{questionId}")]
        public async Task<IActionResult> CountLikes(Guid questionId)
        {
            var likeCount = await _context.Likes.CountAsync(l => l.QuestionId == questionId);
            return Ok(likeCount);
        }

        [HttpPost("like/{questionId}")]
        [Authorize]
        public async Task<IActionResult> LikeQuestion(Guid questionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userId, out Guid guidUserId))
            {
                return Unauthorized("Invalid User ID");
            }

            var existingLike = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == guidUserId && l.QuestionId == questionId);
            if (existingLike != null)
            {
                return BadRequest("You have already liked this question.");
            }

            var like = new Like { UserId = guidUserId, QuestionId = questionId };
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("unlike/{questionId}")]
        [Authorize]
        public async Task<IActionResult> UnlikeQuestion(Guid questionId)
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

            var like = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.QuestionId == questionId);
            if (like == null)
            {
                return NotFound("Like not found.");
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok();
        }


    }
}
