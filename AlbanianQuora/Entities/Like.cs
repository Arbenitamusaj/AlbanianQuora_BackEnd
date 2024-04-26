namespace AlbanianQuora.Entities
{
    public class Like
    {
        public int LikeId { get; set; }
        public Guid UserId { get; set; }
        public Guid QuestionId { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }
    }
}
