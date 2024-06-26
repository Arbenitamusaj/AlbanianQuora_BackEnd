﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

       
        public virtual ICollection<Comment> Comments { get; set; }
     
        public List<Question> Questions { get; set; }
       
        public ICollection<Like> Likes { get; set; }
 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
