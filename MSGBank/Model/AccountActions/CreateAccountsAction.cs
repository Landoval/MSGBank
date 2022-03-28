using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model.AccountActions
{
    public class CreateAccountsAction : AccountAction
    {
        [JsonIgnore]
        private Account Account;
        public CreateAccountsAction(Account account, User user) : base(DateTime.Now, user)
        {
            Message = $"CreateAccountsAction AccountId: {account.Id} User: {User} DateTime {DateTime}";
            Account = account;
        }
        public override object Evaluate()
        {
            Account.AccountActions.Add(this.DateTime, this);
            Account.Customer.Accounts.Add(Account);

            return null;
        }
    }
}
