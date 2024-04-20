using System;
using System.Linq;
using System.Threading.Tasks;
using AlbanianQuora.Data;
using AlbanianQuora.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlbanianQuora.Controllers
{
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
        public async Task<IActionResult> PostQuestionCategory(QuestionCategory questionCategory)
        {
            _context.QuestionCategories.Add(questionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuestionCategory", new { id = questionCategory.Id }, questionCategory);
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
