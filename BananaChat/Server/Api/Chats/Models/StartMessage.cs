using System.Diagnostics.CodeAnalysis;
using FluentValidation;

namespace Server.Api.Chats.Models
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class StartMessage : Message
    {
        public string Message { get; set; }
        public string Id { get; set; }

        public override void Validate()
        {
            new Validator().ValidateAndThrow(this);
        }

        private class Validator : AbstractValidator<StartMessage>
        {
            public Validator()
            {
                RuleFor(message => message.Message).NotEmpty();
            }
        }
    }
}