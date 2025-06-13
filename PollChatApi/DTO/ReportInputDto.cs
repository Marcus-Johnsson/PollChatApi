namespace PollChatApi.DTO
{
    public class ReportInputDto
    {
        public int ObjectId { get; set; }

        public string AdminId { get; set; }

        public string ObjectType { get; set; }

        public bool Toggle { get; set; } 
    }
}
