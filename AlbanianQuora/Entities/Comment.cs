using System;
using System.Diagnostics;

namespace AlbanianQuora.Entities
{
    public class Comment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

