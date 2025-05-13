using PollChatApi.Models;

namespace SubjectWars.Data.Dto
{
    public class CommentCount
    {
        public MainThread Thread { get; set; }

        public int CommentsToday { get; set; }
    }
}
