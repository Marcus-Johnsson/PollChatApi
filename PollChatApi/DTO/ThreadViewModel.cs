using PollChatApi.Model;

namespace PollChatApi.DTO
{
    public class ThreadViewModel
    {
        public MainThreadDto Thread { get; set; }
        public List<CommentDto>? CommentsTree { get; set; } = new();
    }
}
