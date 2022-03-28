using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model.AccountActions
{
    public class GetAccountsAction : AccountAction
    {
        [JsonIgnore]
        private Customer Customer;
        public GetAccountsAction(Customer customer, User user) : base(DateTime.Now, user)
        {
            Message = $"GetAccountsAction User: {User} DateTime {DateTime}";
            Customer = customer;
        }
        public override object Evaluate()
        {
            Customer.Accounts.ForEach(x => x.AccountActions.Add(this.DateTime, this));

            return Customer.Accounts;
        }
    }
}
