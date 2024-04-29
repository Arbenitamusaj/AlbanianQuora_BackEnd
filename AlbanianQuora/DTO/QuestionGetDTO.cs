using System;

namespace AlbanianQuora.DTO
{
    public class QuestionGetDTO
    {
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category { get; set; }
        public string UserName { get; set; }
        public int Views { get; set; }
        public string TimeAgo { get; set; }
    }
}
