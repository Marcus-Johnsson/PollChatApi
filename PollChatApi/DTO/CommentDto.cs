namespace PollChatApi.DTO
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int? ParentCommentId { get; set; }
        public string UserId { get; set; }
        public int ThreadId { get; set; }

        public string? Text { get; set; }

        public string? UserName { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime Date { get; set; }

        public List<CommentDto> Replies { get; set; } = new();

        public bool RemovedByAdmin { get; set; }

        public bool RemovedByUser { get; set; }


        public int? DepthValue { get; set; }

    }
}
