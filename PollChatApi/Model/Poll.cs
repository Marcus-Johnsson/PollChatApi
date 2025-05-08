namespace PollChatApi.Model
{
   
        public class Poll
        {
        public int Id { get; set; }

        public List<Subject> Subjects { get; set; } = new();

        public DateTime Date { get; set; }

        public int? WinnerId { get; set; }

        public List<Vote> Votes { get; set; } = new();
    }
    
}
