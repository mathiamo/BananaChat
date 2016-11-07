namespace Server.Api.Chats.Models
{
    public class ChatSummary
    {
        public string ChatId { get; set; }
        public string CustomerName { get; set; }
        public int WaitedForSeconds { get; set; }
    }
}