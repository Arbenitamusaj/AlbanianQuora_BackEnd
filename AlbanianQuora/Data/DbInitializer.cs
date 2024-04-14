using AlbanianQuora.Entities;

namespace AlbanianQuora.Data
{
    public static class DbInitializer
    {
        public static void Initialize(QuestionContext context)
        {
            if (context.Questions.Any()) return;

            var questions = new List<Question>
            {
                new Question()
                {
                    QuestionDescription = "What's your favorite movie?",
                    LikesCount = 120,
                    AddComment = "I love The Shawshank Redemption!",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite food?",
                    LikesCount = 80,
                    AddComment = "I can never say no to pizza!",
                },

                new Question()
                {
                    QuestionDescription = "Have you ever traveled abroad?",
                    LikesCount = 60,
                    AddComment = "Yes, I went to Japan last year. It was amazing!",
                },

                new Question()
                {
                    QuestionDescription = "Do you enjoy reading?",
                    LikesCount = 90,
                    AddComment = "Absolutely! Books are my escape.",
                },

                new Question()
                {
                    QuestionDescription = "Are you a morning person?",
                    LikesCount = 70,
                    AddComment = "Definitely not, I need at least three cups of coffee to function.",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite hobby?",
                    LikesCount = 110,
                    AddComment = "I love painting. It's so therapeutic.",
                },

                new Question()
                {
                    QuestionDescription = "Do you like to exercise?",
                    LikesCount = 100,
                    AddComment = "Yes, I feel so energized after a good workout!",
                },

                new Question()
                {
                    QuestionDescription = "What's your dream travel destination?",
                    LikesCount = 150,
                    AddComment = "I've always wanted to visit the Maldives!",
                },

                new Question()
                {
                    QuestionDescription = "Do you prefer cats or dogs?",
                    LikesCount = 140,
                    AddComment = "Dogs all the way! They're so loyal.",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite season?",
                    LikesCount = 130,
                    AddComment = "I love autumn. The colors are breathtaking.",
                },

                new Question()
                {
                    QuestionDescription = "Do you believe in aliens?",
                    LikesCount = 110,
                    AddComment = "It's definitely possible. The universe is vast.",
                },

                new Question()
                {
                    QuestionDescription = "What's your go-to karaoke song?",
                    LikesCount = 100,
                    AddComment = "I always sing 'Bohemian Rhapsody' by Queen!",
                },

                new Question()
                {
                    QuestionDescription = "Do you enjoy cooking?",
                    LikesCount = 90,
                    AddComment = "Yes, experimenting with new recipes is so much fun!",
                },

                 new Question()
                {
                    QuestionDescription = "What's your favorite childhood memory?",
                    LikesCount = 80,
                    AddComment = "Building forts with my siblings in the backyard.",
                },

                new Question()
                {
                    QuestionDescription = "Do you believe in fate?",
                    LikesCount = 70,
                    AddComment = "I think everything happens for a reason.",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite book?",
                    LikesCount = 60,
                    AddComment = "To Kill a Mockingbird by Harper Lee. A classic!",
                },

                new Question()
                {
                    QuestionDescription = "Are you a morning person or a night owl?",
                    LikesCount = 50,
                    AddComment = "Definitely a night owl. I do my best work after midnight.",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite type of music?",
                    LikesCount = 40,
                    AddComment = "I love indie rock. It's so raw and authentic.",
                },

                new Question()
                {
                    QuestionDescription = "Do you enjoy hiking?",
                    LikesCount = 30,
                    AddComment = "Yes, being in nature is so refreshing.",
                },

                new Question()
                {
                    QuestionDescription = "What's your favorite holiday?",
                    LikesCount = 20,
                    AddComment = "Christmas, hands down. The decorations, the food, the joy—it's magical.",
                },
            };

            context.Questions.AddRange(questions);

            context.SaveChanges();
        }
    }
}
