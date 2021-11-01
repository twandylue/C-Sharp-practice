using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Respository;

namespace MVC.Controllers
{
    [ApiController]
    public class RestfulAccountController : ControllerBase
    {
        private IAccountData _accountData;
        public RestfulAccountController(IAccountData accountData)
        {
            _accountData = accountData;
        }

        [HttpGet]
        [Route("/api/v1/accounts")]
        public IActionResult GetAccounts()
        {
            return StatusCode((int)HttpStatusCode.OK, this._accountData.GetAccounts());
        }

        [HttpGet]
        [Route("/api/v1/account/{AccountName}")]
        public IActionResult GetAccount(String AccountName)
        {
            var account = this._accountData.GetAccount(AccountName);
            if (account.Id == 0) return StatusCode((int)HttpStatusCode.BadRequest, new { message = "Can't find account." });
            return StatusCode((int)HttpStatusCode.OK, account);
        }

        [HttpPost]
        [Route("/api/v1/addaccount")]
        public IActionResult AddAccount([FromBody] Account account)
        {
            if (this._accountData.GetAccount(account.AccountName).Id == 0)
            {
                this._accountData.AddAccount(account);
                return StatusCode((int)HttpStatusCode.Created, HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + account.AccountName);
            }
            return StatusCode((int)HttpStatusCode.BadRequest, new { message = "Account already be used!" });

        }
        [HttpPost]
        [Route("/api/v1/deleteaccont")]
        public IActionResult DeleteAccount([FromBody] Account account)
        {
            var ExistingAccount = this._accountData.GetAccount(account.AccountName);
            if (ExistingAccount.Id != 0)
            {
                this._accountData.DeleteAccount(ExistingAccount);
                return StatusCode((int)HttpStatusCode.OK, new { message = $"{account.AccountName} was deleted." });
            }
            return StatusCode((int)HttpStatusCode.NotFound, new { message = "Can't find account" });
        }
    }
}