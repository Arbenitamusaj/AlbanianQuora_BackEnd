﻿using AlbanianQuora.Data;
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
    [Route("[controller]")]
    public class CommentController(UserDbContext context) : ControllerBase
    {
        private readonly UserDbContext _context = context;

        [HttpPost("comments/{questionId}")]
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

        [HttpGet("byQuestion/{questionId}")]
       
        public async Task<IActionResult> GetCommentsByQuestionId(Guid questionId)
        {
            var comments = await _context.Comments
                .Where(c => c.QuestionId == questionId)
                .Select(c => new CommentGetDTO
                {
                    UserName = c.User.FirstName,
                    Content = c.Content,
                    TimeAgo = c.CreatedAt.ToString("o") // Send the raw date, formatting can be done on the client
                })
                .ToListAsync();

            return Ok(comments);
        }






    }
}
