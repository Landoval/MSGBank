using Microsoft.AspNetCore.Mvc;
using MSGBank.Data;
using MSGBank.Data.AccountActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Manager
{
    public class BankManager : IBankManager
    {
        private readonly IDatabase Database;
        private Dictionary<Guid, User> SessionID;
        private Dictionary<User, Guid> LogginUsers;
        public BankManager(IDatabase database)
        {
            Database = database;
            SessionID = new();
            LogginUsers = new();
        }

        public IActionResult CreateAccount(string customername, double deposit, User user)
        {
            var customer = Database.FindCustomer(customername);
            if(customer is null)
            {
                return new BadRequestObjectResult($"Customer {customername} not found");
            }
            if (deposit < 0)
            {
                return new BadRequestObjectResult($"Doposit must be positiv. Deposit: {deposit}");
            }
            if (user is null)
            {
                return new BadRequestObjectResult($"User is null");
            }

            Account account = new Account(deposit, customer);
            Database.AddAccount(account);

            CreateAccountsAction action = new CreateAccountsAction(account, user);
            action.Evaluate();
            return new OkResult();
        }

        public IActionResult GetAccounts(string customername, User user)
        {
            var customer = Database.FindCustomer(customername);
            if (customer is null)
            {
                return new BadRequestObjectResult($"Customer not found {customername}");
            }
            if (user is null)
            {
                return new BadRequestObjectResult($"User is null");
            }
            GetAccountsAction action = new GetAccountsAction(customer, user);
            var result = action.Evaluate() as List<Account>;
            return new OkObjectResult(result);
        }

        public IActionResult GetBalance(Guid accountId, User user)
        {
            Account account = Database.FindAccount(accountId);
            if (account is null)
            {
                return new BadRequestObjectResult($"Account not found {accountId}");
            }
            if (user is null)
            {
                return new BadRequestObjectResult($"User is null");
            }
            BalanceAction action = new BalanceAction(account, user);
            var result = (double)action.Evaluate();
            return new OkObjectResult(result);
        }

        public IActionResult History<T>(Guid accountId, User user) where T : AccountAction
        {
            Account account = Database.FindAccount(accountId);
            if (account is null)
            {
                return new BadRequestObjectResult($"Account not found {accountId}");
            }
            if (user is null)
            {
                return new BadRequestObjectResult($"User is null");
            }

            HistoryAction action = new HistoryAction(account,user);
            var result = action.Evaluate() as SortedList<DateTime, AccountAction>;

            var returnVal = result.Where(x => x.Value.GetType().Equals(typeof(T)) || x.Value.GetType().IsSubclassOf(typeof(T)));

            return new OkObjectResult(returnVal);
        }

        public bool IsSessionValid(Guid sessionID)
        {
            return SessionID.ContainsKey(sessionID);
        }

        public IActionResult Transfer(Guid source, Guid target, double amount, User user)
        {
            if(target == source)
            {
                return new BadRequestObjectResult($"Source and Target are equal");
            }
            var sourceAccount = Database.FindAccount(source);
            if (sourceAccount is null)
            {
                return new BadRequestObjectResult($"Invalid source {source}");
            }
            var targetAccount = Database.FindAccount(target);
            if (targetAccount is null)
            {
                return new BadRequestObjectResult($"Invalid target {target}");
            }
            if (amount <= 0)
            {
                return new BadRequestObjectResult($"Bad ammount {amount}");
            }

            if (sourceAccount.Balance + sourceAccount.Credit < amount)
            {
                return new BadRequestObjectResult($"Not enough ammount {amount}");
            }
            if (user is null)
            {
                return new BadRequestObjectResult($"User is null");
            }

            TransferAction action = new TransferAction(sourceAccount,targetAccount, amount ,user);
            action.Evaluate();

            return new OkResult();
        }

        public Guid? Login(string username)
        {

            User user = Database.FindUser(username);
            if (user is null)
                return null;

            if(LogginUsers.ContainsKey(user))
            {
                return LogginUsers[user];
            }
            //Guid sessionId = new Guid("f90cb825-fbba-41c6-bf09-73aac2d936f6");
            Guid sessionId =  Guid.NewGuid();

            SessionID.Add(sessionId, user);
            LogginUsers.Add(user, sessionId);

            return sessionId;
        }

        public bool Logout(Guid sessionID)
        {
            if(SessionID.ContainsKey(sessionID))
            {
                LogginUsers.Remove(SessionID[sessionID]);
                SessionID.Remove(sessionID);
                return true;
            }
            return false;
        }

        public User FindLogedinUser(Guid sessionID)
        {
            if (!SessionID.ContainsKey(sessionID))
                return null;
            return SessionID[sessionID];
        }
    }
}
