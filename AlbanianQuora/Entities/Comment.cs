using System;
using System.Diagnostics;

namespace AlbanianQuora.Entities
{
	public class Comment
	{
        public int Id { get; set; }

        public string Body { get; set; }

        public User Author { get; set; }

        public Question Question { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

