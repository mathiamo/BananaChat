using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Akka;
using Akka.Actor;
using Server.Chats.Commands;

namespace Server.Chats
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Chat : UntypedActor
    {
        private readonly Dictionary<string, IActorRef> chats;

        public Chat()
        {
            chats = new Dictionary<string, IActorRef>();
        }

        protected override void OnReceive(object message)
        {
            message.Match()
                   .With<Start>(StartChat)
                   .With<SendCustomerMessage>(SendMessage)
                   .With<SendSupportMessage>(SendMessage)
                   .With<Join>(JoinChat)
                   .With<Close>(CloseChat)
                ;
        }

        private void CloseChat(Close close)
        {
            Context.System.Stop(chats[close.ChatId]);
            chats.Remove(close.ChatId);
        }

        private void StartChat(Start start)
        {
            chats[start.ChatId] = Context.ActorOf(CustomerChat.Create(start));
        }

        private void JoinChat(Join message)
        {
            chats[message.ChatId].Forward(message);
        }

        private void SendMessage(SendCustomerMessage message)
        {
            chats[message.ChatId].Forward(message);
        }

        private void SendMessage(SendSupportMessage message)
        {
            chats[message.ChatId].Forward(message);
        }

        public class Start : IChatCommand
        {
            public string ChatId { get; }
            public string CustomerName { get; }

            public Start(string chatId, string customerName)
            {
                ChatId = chatId;
                CustomerName = customerName;
            }
        }

        public class Join : IChatCommand
        {
            public string ChatId { get; }

            public Join(string chatId)
            {
                ChatId = chatId;
            }
        }

        public class Close : IChatCommand
        {
            public string ChatId { get; }

            public Close(string chatId)
            {
                ChatId = chatId;
            }
        }

        public class SendCustomerMessage : IChatCommand
        {
            public string ChatId { get; }
            public string Text { get; }

            public SendCustomerMessage(string chatId, string text)
            {
                ChatId = chatId;
                Text = text;
            }
        }

        public class SendSupportMessage : IChatCommand
        {
            public string ChatId { get; set; }
            public string Text { get; set; }

            public SendSupportMessage(string chatId, string text)
            {
                ChatId = chatId;
                Text = text;
            }
        }
    }
}