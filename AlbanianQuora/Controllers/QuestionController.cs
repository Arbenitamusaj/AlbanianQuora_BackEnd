using AlbanianQuora.DTO;
using AlbanianQuora.Data;
using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace AlbanianQuora.Controllers
{
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
            return Ok(await _context.Questions.ToListAsync());
        }


        [HttpPost]
        public async Task<IActionResult> PostQuestion([FromBody] QuestionPostDTO questionDto)
        {
            if (questionDto == null)
            {
                return BadRequest("Invalid question data");
            }

            var question = new Question
            {
                QuestionTitle = questionDto.QuestionTitle,
                QuestionDescription = questionDto.QuestionDescription,
                QuestionCategoryId = questionDto.QuestionCategoryId,
                CreatedAt = DateTime.UtcNow  
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
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
}