namespace PollChatApi.DTO
{
    public class ThreadWithImageDto
    {
        public string UserId { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public IFormFile? Image { get; set; }
    }
}
