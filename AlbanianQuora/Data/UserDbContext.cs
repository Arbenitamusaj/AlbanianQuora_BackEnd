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

        public DbSet<Like> Likes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasOne(q => q.QuestionCategory)  
                .WithMany(c => c.Questions)      
                .HasForeignKey(q => q.QuestionCategoryId);
            modelBuilder.Entity<Question>()
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.UserId);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Question)    
                .WithMany(q => q.Comments)   
                .HasForeignKey(c => c.QuestionId);
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)         
                .WithMany(u => u.Comments)  
                .HasForeignKey(c => c.UserId);
            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Question)
                .WithMany(q => q.Likes)
                .HasForeignKey(l => l.QuestionId);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.UserId, l.QuestionId })
                .IsUnique();
        }
    }
}