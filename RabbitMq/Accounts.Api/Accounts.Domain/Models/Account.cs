using Newtonsoft.Json;

using System;

namespace Accounts.Domain.Models
{
    public class Account
    {
        public Guid Id { get; protected set; }

        public string Name { get; set; }

        [JsonConstructor]
        public Account(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
