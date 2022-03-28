using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Model
{
    public class Customer
    {
        public List<Account> Accounts { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Customer(int id, string name)
        {
            Id = id;
            Name = name;
            Accounts = new();
        }

        internal void AddAccount(Account account)
        {
            Accounts.Add(account);
        }
    }
}
