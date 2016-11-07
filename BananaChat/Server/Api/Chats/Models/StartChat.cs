namespace Server.Api.Chats.Models
{
    public class StartChat : Message
    {
        public string ChatId { get; set; }
        public string CustomerName { get; set; }
        public override void Validate() {}
    }
}