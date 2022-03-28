using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model.AccountActions
{
    public class TransferAction : AccountAction
    {
        [JsonIgnore]
        private Account Source;
        [JsonIgnore]
        private Account Target;
        [JsonIgnore]
        private double Amount;
        public TransferAction(Account source, Account target, double amount, User user) : base(DateTime.Now, user)
        {
            Message = $"TransferAction Source {source.Id} Target {target.Id} Ammount {amount} User: {User} DateTime {DateTime}";
            Source = source;
            Target = target;
            Amount = amount; 
        }
        public override object Evaluate()
        {
            Source.AccountActions.Add(this.DateTime, this);

            Source.Balance -= Amount;

            Target.AccountActions.Add(this.DateTime, this);

            Target.Balance += Amount;

            return null;
        }
    }
}
