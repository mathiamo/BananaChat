using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Akka.Actor;
using Server.Api.Chats.Models;
using Server.Chats;
using Server.Chats.Queries;

namespace Server.Api.Chats
{
    [RoutePrefix("api/chat")]
    public class ChatController : ApiController
    {
        private readonly IActorRef router;

        public ChatController()
        {
            router = ChatContext.MessageRouter;
        }

        [Route("start")]
        public void PostStartChat(FormDataCollection form)
        {
            var start = form.GetMessage<StartChat>();
            router.Tell(new Chat.Start(start.ChatId, start.CustomerName));
        }

        [Route("customer-message")]
        public void PostCustomerMessage(FormDataCollection form)
        {
            var message = form.GetMessage<CustomerMessage>();
            router.Tell(new Chat.SendCustomerMessage(message.ChatId, message.Text));
        }

        [Route("new-messages")]
        [NoCache]
        public async Task<ChatMessage[]> GetNewChatMessages(string chatId, int skip = 0)
        {
            try
            {
                var received = await Ask<ChatQuery.MessagesReady>(new ChatQuery.GetNewMessages(chatId, skip));
                return received.Messages;
            }
            catch (TaskCanceledException)
            {
                return new ChatMessage[0];
            }
        }

        [Route("all-messages")]
        [NoCache]
        public async Task<ChatMessage[]> GetAllChatMessages(string chatId)
        {
            try
            {
                var received = await Ask<ChatQuery.MessagesReady>(new ChatQuery.GetAllMessages(chatId));
                return received.Messages;
            }
            catch (TaskCanceledException)
            {
                return new ChatMessage[0];
            }
        }

        [Route("close")]
        public void PostCloseChat(FormDataCollection form)
        {
            var close = form.GetMessage<CloseChat>();
            router.Tell(new Chat.Close(close.ChatId));
        }

        [Route("list")]
        [NoCache]
        public async Task<ChatSummary[]> GetChatList()
        {
            try
            {
                var received = await Ask<ChatQuery.ListReady>(new ChatQuery.GetList());
                return received.Chats;
            }
            catch (TaskCanceledException)
            {
                return new ChatSummary[0];
            }
        }

        [Route("join")]
        public void PostJoinChat(FormDataCollection form)
        {
            var join = form.GetMessage<JoinChat>();
            router.Tell(new Chat.Join(join.ChatId));
        }

        [Route("support-message")]
        public void PostSupportMessage(FormDataCollection form)
        {
            var message = form.GetMessage<SupportMessage>();
            router.Tell(new Chat.SendSupportMessage(message.ChatId, message.Text));
        }

        private Task<TResponse> Ask<TResponse>(object message)
        {
            var timeout = TimeSpan.FromSeconds(10);
            return router.Ask<TResponse>(message, timeout);
        }
    }
}