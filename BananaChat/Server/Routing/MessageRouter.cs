using Akka;
using Akka.Actor;
using Server.Chats.Commands;
using Server.Chats.Queries;

namespace Server.Routing
{
    internal class MessageRouter : UntypedActor
    {
        private readonly IActorRef commands;
        private readonly IActorRef queries;

        public MessageRouter()
        {
            commands = Context.ActorOf<CommandRouter>();
            queries = Context.ActorOf<QueryRouter>();
        }

        protected override void OnReceive(object message)
        {
            message.Match()
                   .With<ICommand>(Forward)
                   .With<IQuery>(Forward)
                ;
        }

        private void Forward(IQuery query)
        {
            queries.Forward(query);
        }

        private void Forward(ICommand command)
        {
            commands.Forward(command);
        }
    }
}