using Microsoft.AspNetCore.Mvc;
using MSGBank.Data;
using MSGBank.Data.AccountActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Manager
{
    public interface IBankManager
    {
        Guid? Login(string username);
        bool Logout(Guid sessionID);
        bool IsSessionValid(Guid sessionID);
        IActionResult CreateAccount(string customername, double deposit, User user);
        IActionResult GetAccounts(string customername, User user);
        IActionResult GetBalance(Guid accountId, User user);
        IActionResult Transfer(Guid source, Guid target, double amount, User user);
        IActionResult History<T>(Guid accountId, User user) where T : AccountAction;
        User FindLogedinUser(Guid sessionID);
    }
}
