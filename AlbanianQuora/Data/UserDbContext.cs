using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AlbanianQuora.Entities;

namespace AlbanianQuora.Data
{
    public class UserDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public UserDbContext(DbContextOptions<UserDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // If not configured already
            if (!options.IsConfigured)
            {
                options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        public DbSet<User> Users { get; set; }
    }
}
