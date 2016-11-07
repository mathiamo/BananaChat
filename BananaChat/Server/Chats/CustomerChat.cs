using System;
using System.Diagnostics.CodeAnalysis;
using Akka;
using Akka.Actor;
using Server.Chats.Data;
using Server.Chats.Queries;

namespace Server.Chats
{
    public class CustomerChat : UntypedActor
    {
        public static Props Create(Chat.Start start)
        {
            return Props.Create(() => new CustomerChat(start.ChatId, start.CustomerName));
        }

        private readonly string chatId;

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public CustomerChat(string chatId, string customerName)
        {
            this.chatId = chatId;

            var room = ChatRoom.Get(chatId);
            room.CustomerName = customerName;
        }

        protected override void OnReceive(object message)
        {
            message
                .Match()
                .With<Chat.SendCustomerMessage>(SendMessage)
                .With<Chat.SendSupportMessage>(SendMessage)
                .With<Chat.Join>(JoinChat)
                ;
        }

        private void JoinChat(Chat.Join message)
        {
            if (chatId != message.ChatId) return;

            var room = ChatRoom.Get(message.ChatId);
            room.Joined = true;

            ChatContext.MessageRouter.Tell(new ChatQuery.ReturnMessages(message.ChatId));
        }

        private void SendMessage(Chat.SendCustomerMessage message)
        {
            if (chatId != message.ChatId) return;

            var room = ChatRoom.Get(message.ChatId);
            room.AddCustomerMessage(message.Text);

            ChatContext.MessageRouter.Tell(new ChatQuery.ReturnMessages(message.ChatId));
        }

        private void SendMessage(Chat.SendSupportMessage message)
        {
            if (chatId != message.ChatId) return;

            var room = ChatRoom.Get(message.ChatId);
            room.AddSupportMessage(message.Text);

            ChatContext.MessageRouter.Tell(new ChatQuery.ReturnMessages(message.ChatId));
        }
    }
}