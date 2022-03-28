using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSGBank.Data.AccountActions;
using MSGBank.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    ///https://localhost:44392/api/Accounting
    public class AccountingController : ControllerBase
    {
        private readonly ILogger<AccountingController> _logger;
        private readonly IBankManager BankManager;

        public AccountingController(ILogger<AccountingController> logger, IBankManager bankManager)
        {
            _logger = logger;
            BankManager = bankManager;
        }

        [HttpPost]
        [Route("CreateAccount")]
        public IActionResult CreateAccount(Guid sessionID, string customername, double deposit)
        {
            if(!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            IActionResult result = BankManager.CreateAccount(customername, deposit, BankManager.FindLogedinUser(sessionID));
            return result;
        }

        [HttpGet]
        [Route("Accounts")]
        public IActionResult GetAccounts(Guid sessionID, string customername)
        {
            if (!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            return BankManager.GetAccounts(customername, BankManager.FindLogedinUser(sessionID));
        }

        [HttpGet]
        [Route("Balance")]
        public IActionResult GetBalance(Guid sessionID, Guid accountId)
        {
            if (!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            return BankManager.GetBalance(accountId, BankManager.FindLogedinUser(sessionID));
        }

        [HttpPost]
        [Route("Transfer")]
        public IActionResult Transfer(Guid sessionID, Guid source, Guid target, double amount)
        {
            if (!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            return BankManager.Transfer(source, target, amount, BankManager.FindLogedinUser(sessionID));
        }

        [HttpGet]
        [Route("History")]
        public IActionResult History(Guid sessionID, Guid accountId)
        {
            if (!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            return BankManager.History<AccountAction>(accountId, BankManager.FindLogedinUser(sessionID));
        }
        [HttpGet]
        [Route("TransferHistory")]
        public IActionResult TransferHistory(Guid sessionID, Guid accountId)
        {
            if (!BankManager.IsSessionValid(sessionID))
            {
                return Unauthorized();
            }
            return BankManager.History<TransferAction>(accountId, BankManager.FindLogedinUser(sessionID));
        }
    }
}
