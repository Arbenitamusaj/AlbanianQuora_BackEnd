using System;
namespace AlbanianQuora.Entities
{
    public class Question
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public Guid QuestionCategoryId { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

