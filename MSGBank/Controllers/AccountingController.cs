using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSGBank.Model.AccountActions;
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
        private readonly ILogger<AccountingController> _logger; //not used for now
        private readonly IBankManager BankManager;  //resposible for Bank managie interactions

        public AccountingController(ILogger<AccountingController> logger, IBankManager bankManager)
        {
            _logger = logger;
            BankManager = bankManager;
        }

        /// <summary>
        /// Create a new account for an existing Customer
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="customername">Existing username</param>
        /// <param name="deposit">positiv starting deposit</param>
        /// <returns>CreateAccount with info text</returns>
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
        /// <summary>
        /// Overview of all acount Information for one Customer
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="customername">Existing username</param>
        /// <returns></returns>
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
        /// <summary>
        /// Blacane for a given account
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="accountId">account Id</param>
        /// <returns>Balanceinformation</returns>
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
        /// <summary>
        /// Transfer a given amount from source to target account
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="source">source account to deduce from</param>
        /// <param name="target">target account to add to</param>
        /// <param name="amount">transferamount</param>
        /// <returns></returns>
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
        /// <summary>
        /// Complet History of all actions on an account
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="accountId">acount to inspect</param>
        /// <returns></returns>
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

        /// <summary>
        /// History of all transfer actions on a given account
        /// </summary>
        /// <param name="sessionID">activ sessionID</param>
        /// <param name="accountId">account to inspect</param>
        /// <returns></returns>
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
