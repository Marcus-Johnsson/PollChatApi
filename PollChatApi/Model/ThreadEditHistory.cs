namespace PollChatApi.Model
{
    public class ThreadEditHistory
    {
        public int Id { get; set; }

        public int ThreadId { get; set; }

        public string OldTitle { get; set; } = string.Empty;

        public string OldContent { get; set; } = string.Empty;

        public DateTime EditedTime { get; set; } = DateTime.UtcNow;

        public string EditedByUserId { get; set; } = string.Empty;

        public int SubjectId { get; set; }

        public int? SubCategoryId { get; set; }

        public virtual MainThread Thread { get; set; } = null!;
    }
}
