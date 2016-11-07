namespace Server.Api.Chats.Models
{
    public class CloseChat : Message {
        public override void Validate() {}
        public string ChatId { get; set; }
    }
}