using PollChatApi.Model;

namespace PollChatApi.DTO
{
    public class ThreadFilterResult
    {
        public List<MainThreadDto> Threads { get; set; }
        public string? NextCursor { get; set; }
        public bool HasMore { get; set; }
    }
}
