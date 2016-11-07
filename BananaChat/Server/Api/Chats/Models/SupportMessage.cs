namespace Server.Api.Chats.Models
{
    public class SupportMessage : Message {
        public string ChatId { get; set; }
        public string Text { get; set; }

        public override void Validate() {}
    }
}