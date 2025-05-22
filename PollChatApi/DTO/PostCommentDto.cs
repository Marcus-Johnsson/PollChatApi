namespace PollChatApi.DTO
{
    public class PostCommentDto
    {
        public int ThreadId { get; set; }

        public string UserId { get; set; }

        public string Comment { get; set; }

        public int? ParentCommentId { get; set; }

        public bool MainComment { get; set; }
    }
}
