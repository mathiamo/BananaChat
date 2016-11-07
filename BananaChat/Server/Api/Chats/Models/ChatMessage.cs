namespace Server.Api.Chats.Models
{
    public class ChatMessage
    {
        public bool IsCustomer { get; set; }
        public bool IsSupport { get; set; }
        public string Text{ get; set; }
    }
}