using System;
using Akka.Actor;
using Server.Routing;

namespace Server.Chats
{
    internal static class ChatContext
    {
        public static IActorRef MessageRouter { get; set; }

        public static void Start()
        {
            system = ActorSystem.Create("chat");
            MessageRouter = system.ActorOf<MessageRouter>();
        }

        public static void End()
        {
            system.Terminate().Wait(TimeSpan.FromSeconds(20));
        }

        private static ActorSystem system;
    }
}