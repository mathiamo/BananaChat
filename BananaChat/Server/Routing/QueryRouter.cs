using Akka;
using Akka.Actor;
using Server.Chats.Queries;

namespace Server.Routing
{
    internal class QueryRouter : UntypedActor
    {
        private readonly IActorRef chat;

        public QueryRouter()
        {
            chat = Context.ActorOf<ChatQuery>();
        }

        protected override void OnReceive(object message)
        {
            message.Match().With<IChatQuery>(query => chat.Forward(message));
        }
    }
}