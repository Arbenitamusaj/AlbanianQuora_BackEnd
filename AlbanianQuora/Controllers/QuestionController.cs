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
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly UserDbContext _context;

        public QuestionController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet("questions")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions
                .Include(q => q.QuestionCategory) // Ensure the category is loaded
                .Include(q => q.User) // If user details are not loaded by default
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();


            return Ok(questions);
        }

        [HttpGet("question/{categoryId}")]
        public async Task<IActionResult> GetQuestionsByCategory(Guid categoryId)
        {
            var questions = await _context.Questions
                .Where(q => q.QuestionCategoryId == categoryId) 
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(questions);
        }

        [HttpPost("question")]
        [Authorize] 
        public IActionResult PostQuestion([FromBody] QuestionPostDTO questionDTO)
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

            var newQuestion = new Question
            {
                UserId = userId,
                QuestionTitle = questionDTO.QuestionTitle,
                QuestionDescription = questionDTO.QuestionDescription,
                QuestionCategoryId = questionDTO.QuestionCategoryId
            };

            _context.Questions.Add(newQuestion);
            _context.SaveChanges();

            return Ok(new { Message = "Question posted successfully" });
        }

        [HttpGet("question-details/{id}")]
        public async Task<IActionResult> GetQuestion(Guid id)
        {
            var question = await _context.Questions
                .Where(q => q.Id == id)
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,  
                    UserName = q.User.FirstName,  
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")  
                })
                .FirstOrDefaultAsync();

            if (question == null)
            {
                return NotFound("Question not found.");
            }

            return Ok(question);
        }


        [HttpPut("question/{id}")]
        public async Task<IActionResult> PutQuestion(Guid id, Question question)
        {
            if (id != question.Id)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("question/{id}")]
        public async Task<IActionResult> DeleteQuestion(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("question/title/search")]
        public async Task<IActionResult> SearchQuestions(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search term is required.");
            }

            var searchLower = search.ToLower();

            var questions = await _context.Questions
                .Where(q => q.QuestionTitle.ToLower().Contains(searchLower)) 
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(questions);
        }



        [HttpGet("question/mostCommented")]
        public async Task<IActionResult> GetMostCommentedQuestions()
        {
            var questions = await _context.Questions
                .Include(q => q.Comments)  
                .OrderByDescending(q => q.Comments.Count)
                .Take(10)
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o") 
                })
                .ToListAsync();

            return Ok(questions);
        }

        [HttpGet("question/latest")]
        public async Task<IActionResult> GetLatestQuestions()
        {
            var latestQuestions = await _context.Questions
                .OrderByDescending(q => q.CreatedAt)
                .Take(20)
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(latestQuestions);
        }

        [HttpGet("question/mostViewed")]
        public async Task<IActionResult> GetMostViewedQuestions()
        {
            var latestQuestions = await _context.Questions
                .OrderByDescending(q => q.Views)
                .Take(20)
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    Views = q.Views,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(latestQuestions);
        }

        [HttpPost("incrementView/{questionId}")]
        public async Task<IActionResult> IncrementViewCount(Guid questionId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                return NotFound("Question not found");
            }

            question.Views++;  
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        private bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }

    public class QuestionDTO
    {
    }
}