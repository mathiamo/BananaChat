using Akka;
using Akka.Actor;

namespace Server.Chats.Queries
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