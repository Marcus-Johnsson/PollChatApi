namespace PollChatApi.DTO
{
    public class ThreadEditDto
    {
        public int ThreadId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int SubjectId { get; set; }

        public int? SubCategoryId { get; set; }

        public string EditedByUserId { get; set; }
    }
}
