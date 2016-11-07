using Akka;
using Akka.Actor;
using Server.Chats.Data;

namespace Server.Chats.Queries
{
    internal class GetAllMessagesQuery : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            message.Match().With<ChatQuery.GetAllMessages>(get =>
                                                           {
                                                               var room = ChatRoom.Get(get.ChatId);
                                                               var messages = room.Messages;
                                                               var ready = new ChatQuery.MessagesReady(messages);

                                                               Sender.Tell(ready);

                                                               Context.Stop(Self);
                                                           });
        }
    }
}