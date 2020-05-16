
using Accounts.Domain.Models;

using MediatR;

namespace Accounts.Domain.Commands
{
    public class CreateAccountCommand : IRequest<bool>
    {
        public Account Account { get; set; }
    }
}
