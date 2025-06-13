using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PollChatApi.Model
{
    public class Warning
    {
        public int Id { get; set; }

        public string UserId { get; set; }  // reported person

        public int ObjectsId { get; set; }

        public string Describtion { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }


        public string Type { get; set; } //Thread, Comment ....

        public string? AdminId { get; set; }
        public bool IsHandled { get; set; } = false;

        public DateTime? HandeldAtTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Scrap { get; set; }


        [JsonPropertyName("RepoUser")]
        public string RepoUser { get; set; } // the one that made the report

        // Additional bools might come later (More what the warning is about as well to help data collection)
    }
}
