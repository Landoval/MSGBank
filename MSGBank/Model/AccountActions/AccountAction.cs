using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MSGBank.Model.AccountActions
{
    public abstract class AccountAction
    {
        public string Message { get; protected set; }
        [JsonIgnore]
        public DateTime DateTime { get; private set; }

        [JsonIgnore]
        public User User { get; private set; }

        public AccountAction(DateTime DateTime, User user)
        {
            DateTime = DateTime;
            User = user;
        }
        public abstract object Evaluate();

    }
}
