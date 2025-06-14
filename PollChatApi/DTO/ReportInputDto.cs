using System.Text.Json.Serialization;

namespace PollChatApi.DTO
{
    public class ReportInputDto
    {
        [JsonPropertyName("ObjectId")]
        public int ObjectId { get; set; }

        [JsonPropertyName("AdminId")]
        public string AdminId { get; set; }

        [JsonPropertyName("ObjectType")]
        public string ObjectType { get; set; }

        [JsonPropertyName("Toggle")]
        public bool Toggle { get; set; }

        [JsonPropertyName("Action")]
        public string Action { get; set; }

        [JsonPropertyName("WarningId")]
        public int WarningId { get; set; }
    }
}
