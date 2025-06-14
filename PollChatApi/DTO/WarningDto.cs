using System.Text.Json.Serialization;

namespace PollChatApi.DTO
{
    public class WarningDto
    {
        [JsonPropertyName("Id")]
        public int Id { get; set; }
        [JsonPropertyName("UserId")]
        public string UserId { get; set; } // reported person

        [JsonPropertyName("ObjectsId")]
        public int ObjectsId { get; set; }

        [JsonPropertyName("Describtion")]
        public string Describtion { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("HandeldAtTime")]
        public DateTime? HandeldAtTime { get; set; }

        [JsonPropertyName("AdminId")]
        public string? AdminId { get; set; }

        [JsonPropertyName("Scrap")]
        public bool Scrap { get; set; }

        [JsonPropertyName("RepoUser")]
        public string RepoUser { get; set; } //the user that reported


    }
}
