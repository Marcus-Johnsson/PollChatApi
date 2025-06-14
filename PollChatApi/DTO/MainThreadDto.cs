using System.Text.Json.Serialization;

namespace PollChatApi.DTO
{
    public class MainThreadDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("UserId")]
        public string UserId { get; set; }

        [JsonPropertyName("ProfilePicture")]
        public string? ProfilePicture { get; set; }

        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Content")]
        public string? Content { get; set; }

        [JsonPropertyName("ImagePath")]
        public string? ImagePath { get; set; }

        [JsonPropertyName("SubjectId")]
        public int? SubjectId { get; set; }

        public virtual SubjectDto Subject { get; set; } = null!;

        [JsonPropertyName("CommentCount")]
        public int CommentCount { get; set; }

        [JsonPropertyName("RemovedByAdmin")]
        public bool? RemovedByAdmin { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
