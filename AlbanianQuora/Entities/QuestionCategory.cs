using System;
using System.Collections.Generic;
namespace AlbanianQuora.Entities
{
	public class QuestionCategory
	{
        public Guid Id { get; set; }
        public string Category { get; set; }

        public List<Question> Questions { get; set; }
    }


}

