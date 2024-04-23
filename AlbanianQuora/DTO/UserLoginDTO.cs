﻿using System.ComponentModel.DataAnnotations;

namespace AlbanianQuora.DTO
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
