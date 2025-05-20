using PollChatApi.Model;

namespace PollChatApi.DTO
{
    public class ThreadFilterResult
    {
        public List<MainThread> Threads { get; set; }
        public string? NextCursor { get; set; }
        public bool HasMore { get; set; }
    }
}
