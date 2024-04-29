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
    [Route("api")]
    public class CommentController(UserDbContext context) : ControllerBase
    {
        private readonly UserDbContext _context = context;

        [HttpPost("comment/{questionId}")]
        [Authorize]
        public async Task<IActionResult> PostComment(Guid questionId, [FromBody] CommentPostDTO commentDTO)
        {
            // Extract the UserId from the token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("User ID is missing in the token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized("User ID is invalid");
            }

            var comment = new Comment
            {
                Content = commentDTO.Content,
                QuestionId = questionId,
                UserId = userId, 
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(new { comment.Id, message = "Comment created successfully." });
        }

        [HttpGet("comments/{questionId}")]
       
        public async Task<IActionResult> GetCommentsByQuestionId(Guid questionId)
        {
            var comments = await _context.Comments
                .Where(c => c.QuestionId == questionId)
                .Select(c => new CommentGetDTO
                {
                    Id = c.Id,
                    UserId = c.User.UserId,
                    UserName = c.User.FirstName,
                    Content = c.Content,
                    TimeAgo = c.CreatedAt.ToString("o") 
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPut("comment/{commentId}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] CommentPostDTO commentDTO)
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

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if (comment.UserId != userId)
            {
                return BadRequest("You are not authorized to update this comment");
            }

            comment.Content = commentDTO.Content; 

            await _context.SaveChangesAsync();

            return Ok(new { comment.Id, message = "Comment updated successfully." });
        }

        [HttpGet("question/{id}/commentcount")]
        public ActionResult<CommentCountDTO> GetCommentCount(Guid id)
        {
            var commentCount = _context.Comments.Count(c => c.QuestionId == id);

            var result = new CommentCountDTO
            {
                QuestionId = id,
                CommentCount = commentCount
            };

            return Ok(result);
        }

        [HttpDelete("comment/{commentId}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(Guid commentId)
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

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if (comment.UserId != userId)
            {
                return BadRequest("You are not authorized to delete this comment");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok("Comment deleted successfully.");
        }






    }
}
