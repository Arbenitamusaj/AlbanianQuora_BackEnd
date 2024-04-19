using System.ComponentModel.DataAnnotations;
namespace AlbanianQuora.Entities

{
    public class User
    {
        public Guid UserId { get; set; } = Guid.NewGuid();

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
