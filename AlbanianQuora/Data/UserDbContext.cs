using Microsoft.EntityFrameworkCore;
using AlbanianQuora.Entities;


namespace AlbanianQuora.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Other user-related DbSet properties...
    }
}
