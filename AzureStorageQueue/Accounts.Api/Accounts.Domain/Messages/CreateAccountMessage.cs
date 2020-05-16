using Accounts.Domain.Models;

using Herald.MessageQueue;

namespace Accounts.Domain.Messages
{
    public class CreateAccountMessage : MessageBase
    {
        public Account Account { get; set; }

        public CreateAccountMessage(Account account)
        {
            Account = account;
        }
    }
}
