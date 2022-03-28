using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model.AccountActions
{
    public class HistoryAction : AccountAction
    {
        [JsonIgnore]
        private Account Source;
        public HistoryAction(Account source, User user) : base(DateTime.Now, user)
        {
            Message = $"HistoryAction User: {User} DateTime {DateTime}";
            Source = source;
        }
        public override object Evaluate()
        {
            Source.AccountActions.Add(this.DateTime, this);

            return Source.AccountActions;
        }
    }
}
