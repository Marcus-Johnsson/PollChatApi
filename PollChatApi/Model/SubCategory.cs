namespace PollChatApi.Model
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public ICollection<MainThread> Threads { get; set; } = new List<MainThread>();
    }
}
