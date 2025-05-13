namespace PollChatApi.Model
{
    public class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public DateTime Date { get; set; }

        public bool MainComment { get; set; }

        public string? ImagePath { get; set; }

        public ICollection<Comment>? Replies { get; set; }
    }
}
