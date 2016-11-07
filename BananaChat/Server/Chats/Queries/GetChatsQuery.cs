using System;
using System.Linq;
using Akka;
using Akka.Actor;
using Server.Chats.Data;

namespace Server.Chats.Queries
{
    internal class GetChatsQuery : UntypedActor
    {
        private readonly ChatQuery.GetList query;
        private readonly IActorRef sender;

        public GetChatsQuery(ChatQuery.GetList query, IActorRef sender)
        {
            this.query = query;
            this.sender = sender;
        }

        protected override void PreStart()
        {
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromSeconds(3),
                                                      Self,
                                                      new ChatQuery.ReturnChats(),
                                                      ActorRefs.NoSender);
        }

        protected override void OnReceive(object message)
        {
            message.Match().With<ChatQuery.ReturnMessages>(fulfill =>
                                                    {
                                                        var chats = ChatRoom.List();
                                                        var ready = new ChatQuery.ListReady(chats);

                                                        sender.Tell(ready);

                                                        Context.Stop(Self);
                                                    });
        }
    }
}