using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Server.Api.Chats.Models;

namespace Server.Chats.Data
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class ChatRoom
    {
        private static readonly Dictionary<string, ChatRoom> Rooms;

        public static ChatRoom Get(string chatId)
        {
            lock (Rooms)
            {
                ChatRoom room;

                if (Rooms.TryGetValue(chatId, out room)) return room;

                room = new ChatRoom(chatId);
                Rooms.Add(chatId, room);
                return room;
            }
        }

        public static ChatSummary[] List()
        {
            return Rooms.Values
                        .Where(room => !room.Joined)
                        .Select(room => new ChatSummary
                                        {
                                            ChatId = room.ChatId,
                                            CustomerName = room.CustomerName,
                                            WaitedForSeconds = room.WaitedForSeconds
                                        })
                        .OrderByDescending(summary => summary.WaitedForSeconds)
                        .ToArray();
        }

        public string ChatId { get; }

        public string CustomerName { get; set; }
        public ChatMessage[] Messages => messages.ToArray();
        public int WaitedForSeconds => (int) (DateTime.Now - startedAt).TotalSeconds;
        public bool Joined { get; set; }

        private readonly List<ChatMessage> messages;
        private readonly DateTime startedAt;

        static ChatRoom()
        {
            Rooms = new Dictionary<string, ChatRoom>();
        }

        private ChatRoom(string chatId)
        {
            ChatId = chatId;
            messages = new List<ChatMessage>();
            startedAt = DateTime.Now;
        }

        public void AddCustomerMessage(string text)
        {
            var message = new ChatMessage
                          {
                              IsCustomer = true,
                              IsSupport = false,
                              Text = text
                          };

            messages.Add(message);
        }

        public void AddSupportMessage(string text)
        {
            var message = new ChatMessage
                          {
                              IsCustomer = false,
                              IsSupport = true,
                              Text = text
                          };

            messages.Add(message);
        }
    }
}