using PollChatApi.Model;

namespace PollChatApi.DTO
{
    public class UserFavoritesDto
    {
        public string Id { get; set; }
        public List<SubjectDto>? FavoriteSubjects { get; set; }
        public List<MainThreadDto>? FavoriteThreads { get; set; }
    }
}
