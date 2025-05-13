using PollChatApi.Model;
using SubjectWars.Models;

namespace PollChatApi.Models
{
    public class MainThread
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        public int SubjectId { get; set; }
        
        public virtual Subject Subject { get; set; } = null!;

        public int? SubCategoryId { get; set; }
        public virtual SubCategory? SubCategory { get; set; }

        public string UserId { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
