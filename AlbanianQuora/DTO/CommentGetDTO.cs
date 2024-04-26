namespace AlbanianQuora.DTO
{
    public class CommentGetDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string TimeAgo { get; set; }
    }
}
