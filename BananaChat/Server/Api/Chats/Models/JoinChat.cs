namespace Server.Api.Chats.Models
{
    public class JoinChat : Message {
        public string ChatId { get; set; }
        public string SupportNick { get; set; }
        public override void Validate() {}
    }
}