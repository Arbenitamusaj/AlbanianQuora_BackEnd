using System;
using System.Threading.Tasks;
using AlbanianQuora.Data;
using AlbanianQuora.DTO;
using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Http.Cors;

namespace AlbanianQuora.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionCategoryController : ControllerBase
    {
        private readonly UserDbContext _context;

        public QuestionCategoryController(UserDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestionCategories()
        {
            return Ok(await _context.QuestionCategories.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> PostQuestionCategory([FromBody] QuestionCategoryDTO categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Category))
            {
                return BadRequest("Category name is required");
            }

            var questionCategory = new QuestionCategory
            {
                Id = Guid.NewGuid(), 
                Category = categoryDto.Category
            };

            _context.QuestionCategories.Add(questionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestionCategory), new { id = questionCategory.Id }, questionCategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionCategory(Guid id)
        {
            var questionCategory = await _context.QuestionCategories.FindAsync(id);
            if (questionCategory == null)
            {
                return NotFound();
            }
            return Ok(questionCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestionCategory(Guid id, QuestionCategory questionCategory)
        {
            if (id != questionCategory.Id)
            {
                return BadRequest();
            }

            _context.Entry(questionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionCategoryExists(id))
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
        public async Task<IActionResult> DeleteQuestionCategory(Guid id)
        {
            var questionCategory = await _context.QuestionCategories.FindAsync(id);
            if (questionCategory == null)
            {
                return NotFound();
            }

            _context.QuestionCategories.Remove(questionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuestionCategoryExists(Guid id)
        {
            return _context.QuestionCategories.Any(e => e.Id == id);
        }
    }
}
