using System.Text.Json.Serialization;

namespace PollChatApi.DTO
{
    public class MainThreadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string UserId { get; set; }
        public string? ProfilePicture { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public string? ImagePath { get; set; }

        public int? SubjectId { get; set; }

        [JsonPropertyName("subject")]
        public virtual SubjectDto Subject { get; set; } = null!;

        public int CommentCount { get; set; }

        public bool? RemovedByAdmin { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
