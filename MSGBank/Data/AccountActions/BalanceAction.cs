using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Data.AccountActions
{
    public class BalanceAction : AccountAction
    {
        [JsonIgnore]
        private Account Source;
        public BalanceAction(Account source, User user) : base(DateTime.Now, user)
        {
            Source = source;
            Message = $"BalanceAction User: {User} Datetime {DateTime}";
        }
        public override object Evaluate()
        {
            Source.AccountActions.Add(this.DateTime, this);

            return Source.Balance;
        }
    }
}
