namespace PollChatApi.Model
{
    public class Warning
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Describtion { get; set; }

        // Additional bools might come later (More what the warning is about as well to help data collection)
    }
}
