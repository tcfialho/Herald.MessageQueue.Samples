using Accounts.Domain.Commands;
using Accounts.Domain.Messages;

using Herald.MessageQueue;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace Accounts.Domain.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, bool>
    {
        private readonly IMessageQueue _messageQueue;

        public CreateAccountHandler(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;
        }

        public Task<bool> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            _messageQueue.Send(new CreateAccountMessage(request.Account));

            return Task.FromResult(true);
        }
    }
}