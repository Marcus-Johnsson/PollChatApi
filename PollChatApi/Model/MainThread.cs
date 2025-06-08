namespace PollChatApi.Model
{
    public class MainThread
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int SubjectId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;


        public string? ImagePath { get; set; }

        public virtual Subject Subject { get; set; } = null!;
        public virtual User User { get; set; } = null!;


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RemovedAt { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();



    }
}
