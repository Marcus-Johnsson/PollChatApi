namespace PollChatApi.Model
{
    public class Subject
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<Poll> Polls { get; set; } = new();
    }
}
