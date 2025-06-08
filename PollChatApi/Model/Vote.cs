namespace PollChatApi.Model
{
    public class Vote
    {
        public int Id { get; set; }

        public int PollId { get; set; }
        public Poll Poll { get; set; }

        public int SubjectId { get; set; }
        public string UserId { get; set; }

        public DateTime Date { get; set; }
    }
}
