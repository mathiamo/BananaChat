using System;
using System.Linq;
using Akka;
using Akka.Actor;
using Server.Chats.Data;

namespace Server.Chats.Queries
{
    internal class GetNewMessagesQuery : UntypedActor
    {
        private readonly ChatQuery.GetNewMessages query;
        private readonly IActorRef sender;

        public GetNewMessagesQuery(ChatQuery.GetNewMessages query, IActorRef sender)
        {
            this.query = query;
            this.sender = sender;
        }

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3),
                                                      Self,
                                                      new ChatQuery.ReturnMessages(query.ChatId),
                                                      ActorRefs.NoSender);
        }

        protected override void OnReceive(object message)
        {
            message.Match().With<ChatQuery.ReturnMessages>(fulfill =>
                                          {
                                              if (query.ChatId != fulfill.ChatId) return;

                                              var room = ChatRoom.Get(query.ChatId);
                                              var messages = room.Messages.Skip(query.Skip);
                                              var ready = new ChatQuery.MessagesReady(messages);

                                              sender.Tell(ready);

                                              Context.Stop(Self);
                                          });
        }
    }
}