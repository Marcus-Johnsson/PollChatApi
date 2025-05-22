namespace PollChatApi.DTO
{
    public class CommentsEditDto
    {
        public int CommentId { get; set; }

        public string NewContent { get; set; }

        public string EditbyUserId { get; set; }
    }
}
