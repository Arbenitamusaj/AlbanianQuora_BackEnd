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
    public class QuestionController : ControllerBase
    {
        private readonly UserDbContext _context;

        public QuestionController(UserDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions
                .Select(q => new QuestionGetDTO
                {
                    QuestionId = q.Id,
                    Title = q.QuestionTitle,
                    Content = q.QuestionDescription,
                    Category = q.QuestionCategory.Category,
                    UserName = q.User.FirstName,
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(questions);
        }
        [HttpGet("ByCategory/{categoryId}")]
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
                    TimeAgo = q.CreatedAt.ToString("o")
                })
                .ToListAsync();

            return Ok(questions);
        }



        [HttpPost]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestion(Guid id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            return Ok(question);
        }

        [HttpPut("{id}")]
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


        [HttpDelete("{id}")]
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

        private bool QuestionExists(Guid id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }

    public class QuestionDTO
    {
    }
}