using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Server.Api.Chats.Models;

namespace Server
{
    static class MessageConversions
    {
        public static TMessage GetMessage<TMessage>(this FormDataCollection form)
            where TMessage: Message
        {
            var value = form.Get("message");
            var message = JsonConvert.DeserializeObject<TMessage>(value);
            message.Validate();
            return message;
        }
    }
}