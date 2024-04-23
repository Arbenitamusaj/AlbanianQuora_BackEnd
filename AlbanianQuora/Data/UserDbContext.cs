using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AlbanianQuora.Entities;

namespace AlbanianQuora.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<QuestionCategory> QuestionCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionCategory)  
                .WithMany(c => c.Questions)      
                .HasForeignKey(q => q.QuestionCategoryId);  
        }
    }
}