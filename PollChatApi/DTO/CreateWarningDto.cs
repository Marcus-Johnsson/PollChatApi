using System.Text.Json.Serialization;

namespace PollChatApi.DTO
{
    public class CreateWarningDto
    {

        [JsonPropertyName("UserId")]
        public string UserId { get; set; }


        [JsonPropertyName("Report")]
        public string Report { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("ObjectId")]
        public int ObjectId { get; set; }



        [JsonPropertyName("ObjectOwnerId")]
        public string ObjectOwnerId { get; set; }

        [JsonPropertyName("RepoUser")]
        public string RepoUser { get; set; }
    }
}
