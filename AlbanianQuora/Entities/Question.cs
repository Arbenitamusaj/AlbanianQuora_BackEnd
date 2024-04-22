using System;
namespace AlbanianQuora.Entities
{
	public class Question
	{
        public Guid Id { get; set; }

        public string QuestionTitle { get; set; }

        public string QuestionDescription { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }
    }
}

