using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MSGBank.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSGBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ILogger<SessionController> _logger;
        private readonly IBankManager BankManager;

        public SessionController(ILogger<SessionController> logger, IBankManager bankManager)
        {
            _logger = logger;
            BankManager = bankManager;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string username)
        {
            Guid? sessionID = BankManager.Login(username);

            if(sessionID is null)
            {
                return NotFound(username);
            }

            return Ok(sessionID.Value);
        }
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout(Guid sessionID)
        {
            bool ok = BankManager.Logout(sessionID);

            if(ok)
                return Ok();

            return NotFound();
        }
    }
}
