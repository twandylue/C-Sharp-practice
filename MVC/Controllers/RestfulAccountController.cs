using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Respository;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [ApiController]
    public class RestfulAccountController : ControllerBase
    {
        private IAccountData _accountData;
        private ISSOAccount _ssoAccountData;
        public RestfulAccountController(IAccountData accountData, ISSOAccount ssoAccountData)
        {
            _accountData = accountData;
            _ssoAccountData = ssoAccountData;
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
                string urlForBinding = Utility.GetGoogleSSOUrlforSingup(account);
                // return StatusCode((int)HttpStatusCode.Created, HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + account.AccountName);
                return StatusCode((int)HttpStatusCode.Created, new { redirect_Uri = urlForBinding });
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

        [HttpGet]
        [Route("/api/v1/checkPortalSSO")]
        public IActionResult CheckProtalSSO()
        {
            var code = Request.Query["code"];
            Console.WriteLine("code: " + code);
            string rawState = Request.Query["state"];

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(rawState)) return StatusCode((int)HttpStatusCode.BadRequest, new
            {
                message = "query parameter wrong."
            });

            string client_id = "536062935773-e1hvscne4ead0kk62fho999kc179rhhj.apps.googleusercontent.com";
            string client_secret = "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";
            // string redirect_Uri = "https://localhost:5001/";
            string redirect_Uri = "https://localhost:5001/api/v1/checkPortalSSO";

            // get token
            var token = Utility.GetTokenFromCode(code, client_id, client_secret, redirect_Uri);
            Console.WriteLine(JsonConvert.SerializeObject(token, Formatting.Indented));

            // get user info
            var userInfo = Utility.GetUserInfo(token.access_token);
            Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented)); // display

            try
            {
                string state = rawState.Replace("\\\"","\"");
                Console.WriteLine(state);
                StateInfo stateInfo = JsonConvert.DeserializeObject<StateInfo>(state);

                if (stateInfo.type == "Singup")
                {
                    // ! bindingRelation 有待抽象化(GoogleId)
                    sso_account_binding bindingRelationShip = new sso_account_binding
                    {
                        idp = stateInfo.idp,
                        sourceId = userInfo.id,
                        accountId = stateInfo.accountId,
                    };
                    this._ssoAccountData.BindAccount(bindingRelationShip);
                    return StatusCode((int)HttpStatusCode.OK, new { message = "Binding account success." });
                }
                else if (stateInfo.type == "Login")
                {
                    (bool isSuccess, string message) = this._ssoAccountData.LoginSSOAccount(userInfo.id);
                    HttpStatusCode httpStatus = new HttpStatusCode();
                    httpStatus = isSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                    return StatusCode((int)httpStatus, new { message = message });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("state formate in not json");
                return StatusCode((int)HttpStatusCode.BadRequest, new { message = "state formate in not json" });
            }

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}