using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Data.AccountActions
{
    public class GetAccountsAction : AccountAction
    {
        [JsonIgnore]
        private Customer Customer;
        public GetAccountsAction(Customer customer, User user) : base(DateTime.Now, user)
        {
            Message = $"GetAccountsAction User: {User} Datetime {DateTime}";
            Customer = customer;
        }
        public override object Evaluate()
        {
            Customer.Accounts.ForEach(x => x.AccountActions.Add(this.DateTime, this));

            return Customer.Accounts;
        }
    }
}
