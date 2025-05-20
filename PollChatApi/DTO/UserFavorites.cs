using PollChatApi.Model;

namespace PollChatApi.DTO
{
    public class UserFavorites
    {
        public string Id { get; set; }
        public List<SubCategory>? FavoriteSubcategories { get; set; }
        public List<Subject>? FavoriteSubjects { get; set; }
        public List<MainThread>? FavoriteThreads { get; set; }
    }
}
