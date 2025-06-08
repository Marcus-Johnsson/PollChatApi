namespace PollChatApi.DTO
{
    public class CommentDto
    {
        public string UserId { get; set; }

        public string Text { get; set; }

        public string? UserName { get; set; }

        public DateTime Date { get; set; }

        public List<CommentDto> Replies { get; set; } = new();
    }
}
