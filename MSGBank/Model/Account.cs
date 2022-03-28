using MSGBank.Model.AccountActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        public SortedList<DateTime, AccountAction> AccountActions { get; private set; }
        public double Balance { get; set; }
        public double Credit { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

        public Account(double balance, Customer customer) 
        {
            Id = Guid.NewGuid();
            Balance = balance;
            Credit = 0;
            AccountActions = new();
            Customer = customer;
        }
    }
}
