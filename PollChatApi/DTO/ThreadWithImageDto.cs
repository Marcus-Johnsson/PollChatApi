namespace PollChatApi.DTO
{
    public class ThreadWithImageDto
    {
        public string UserId { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Text { get; set; } = string.Empty;

        public string? Image { get; set; }
    }
}
