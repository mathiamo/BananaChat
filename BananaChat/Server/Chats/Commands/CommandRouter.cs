using Akka;
using Akka.Actor;

namespace Server.Chats.Commands
{
    internal class CommandRouter : UntypedActor
    {
        private readonly IActorRef chat;

        public CommandRouter()
        {
            chat = Context.ActorOf<Chat>();
        }

        protected override void OnReceive(object message)
        {
            message.Match().With<IChatCommand>(command => chat.Forward(message));
        }
    }
}