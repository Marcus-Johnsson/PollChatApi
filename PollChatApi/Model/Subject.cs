namespace PollChatApi.Model
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public ICollection<User>? SubscribedUsers { get; set; } 

        public ICollection<Poll>? Polls { get; set; } 
    }
}
