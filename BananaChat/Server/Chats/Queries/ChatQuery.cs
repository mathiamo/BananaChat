using System.Collections.Generic;
using System.Linq;
using Akka;
using Akka.Actor;
using Server.Api.Chats.Models;
using Server.Chats.Data;

namespace Server.Chats.Queries
{
    public class ChatQuery : UntypedActor
    {
        private readonly List<IActorRef> messageQueries;

        public ChatQuery()
        {
            messageQueries = new List<IActorRef>();
        }

        protected override void OnReceive(object message)
        {
            message.Match()
                   .With<GetNewMessages>(QueryLatestMessages)
                   .With<GetAllMessages>(QueryAllMessages)
                   .With<GetList>(QueryChats)
                   .With<ReturnMessages>(FulfillQuery)
                ;
        }

        private void QueryChats(GetList get)
        {
            var chats = ChatRoom.List();
            var ready = new ListReady(chats);

            Sender.Tell(ready);
        }

        private void QueryAllMessages(GetAllMessages get)
        {
            var query = Context.ActorOf(Props.Create(() => new GetAllMessagesQuery()));
            query.Forward(get);
        }

        private void QueryLatestMessages(GetNewMessages get)
        {
            var query = Context.ActorOf(Props.Create(() => new GetNewMessagesQuery(get, Sender)));
            messageQueries.Add(query);
        }

        private void FulfillQuery(ReturnMessages retrieved)
        {
            messageQueries.ForEach(query => query.Tell(retrieved));
            messageQueries.RemoveAll(TerminatedActors);
        }

        private static bool TerminatedActors(IActorRef actor)
        {
            var local = actor as LocalActorRef;
            return local != null && local.IsTerminated;
        }

        public class GetNewMessages : IChatQuery
        {
            public string ChatId { get; }
            public int Skip { get; }

            public GetNewMessages(string chatId, int skip)
            {
                ChatId = chatId;
                Skip = skip;
            }
        }

        public class GetList : IChatQuery { }

        public class MessagesReady : IChatQuery
        {
            public ChatMessage[] Messages { get; }

            public MessagesReady(IEnumerable<ChatMessage> messages)
            {
                Messages = messages.ToArray();
            }
        }

        public class ListReady : IChatQuery
        {
            public ChatSummary[] Chats { get;  }

            public ListReady(ChatSummary[] chats)
            {
                Chats = chats;
            }
        }

        public class GetAllMessages : IChatQuery
        {
            public string ChatId { get; }

            public GetAllMessages(string chatId)
            {
                ChatId = chatId;
            }
        }

        public class ReturnMessages : IChatQuery
        {
            public string ChatId { get; }

            public ReturnMessages(string chatId)
            {
                ChatId = chatId;
            }
        }

        public class ReturnChats : IChatQuery
        {
          
        }
    }
}