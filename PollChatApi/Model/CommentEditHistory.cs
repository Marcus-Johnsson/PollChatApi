namespace PollChatApi.Model
{
    public class CommentEditHistory
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public DateTime EditDate { get; set; }

        public string EditByUserId { get; set; }
    }
}
