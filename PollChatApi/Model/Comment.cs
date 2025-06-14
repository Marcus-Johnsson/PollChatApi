namespace PollChatApi.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public int ThreadId { get; set; }
        public virtual MainThread Thread { get; set; } = null!;

        public string UserId { get; set; }

        public User User { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public DateTime? RemovedAt { get; set; }

        public int? ParentCommentId { get; set; }     // First comment is null
        public virtual Comment? ParentComment { get; set; }

        public bool RemovedByAdmin { get; set; } = false;

        public bool RemovedByUser { get; set; } = false;

        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}
