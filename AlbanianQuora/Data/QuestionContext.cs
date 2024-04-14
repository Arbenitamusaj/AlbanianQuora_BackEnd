using AlbanianQuora.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlbanianQuora.Data
{
    public class QuestionContext : DbContext
    {
        public QuestionContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }
    }
}
