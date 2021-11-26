using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Models.Response;
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
            if (account.Id == 0) return StatusCode((int)HttpStatusCode.BadRequest, new FailedResponse { message = "Can't find the account." });
            return StatusCode((int)HttpStatusCode.OK, account);
        }

        [HttpPost]
        [Route("/api/v1/addaccount")]
        public IActionResult AddAccount([FromBody] Account account)
        {
            if (this._accountData.GetAccount(account.AccountName).Id == 0)
            {
                Account _newAccoutn = this._accountData.AddAccount(account);
                // TODO idp 有待抽象化
                StateInfo stateInfo = new StateInfo
                {
                    idp = 2,
                    type = "Singup",
                    accountId = _newAccoutn.Id
                };
                string SSOUrl = Utility.GetGoogleSSOUrl(stateInfo);
                return StatusCode((int)HttpStatusCode.Created, new SuccessResponse { redirect_Uri = SSOUrl });
            }
            return StatusCode((int)HttpStatusCode.BadRequest, new FailedResponse { message = "Account already be registed!" });
        }

        [HttpPost]
        [Route("/api/v1/getBindingUrl")]
        public IActionResult GetBindingUrl([FromBody] Account account)
        {
            (bool isSuccess, Account _account) = this._accountData.CheckAccount(account);
            if (!isSuccess) return StatusCode((int)HttpStatusCode.BadRequest, new FailedResponse { message = "Account or Password is wrong" });
            // TODO idp 有待抽象化
            StateInfo stateInfo = new StateInfo
            {
                idp = 2,
                type = "Singup",
                accountId = _account.Id
            };
            string SSOUrl = Utility.GetGoogleSSOUrl(stateInfo);
            return StatusCode((int)HttpStatusCode.OK, new SuccessResponse { redirect_Uri = SSOUrl });
        }

        [HttpPost]
        [Route("/api/v1/deleteaccont")]
        public IActionResult DeleteAccount([FromBody] Account account)
        {
            var ExistingAccount = this._accountData.GetAccount(account.AccountName);
            if (ExistingAccount.Id != 0)
            {
                this._accountData.DeleteAccount(ExistingAccount);
                return StatusCode((int)HttpStatusCode.OK, new SuccessResponse { message = $"{account.AccountName} was deleted." });
            }
            return StatusCode((int)HttpStatusCode.NotFound, new FailedResponse { message = "Can't find account" });
        }

        [HttpGet]
        [Route("api/v1/LoginSSO")]
        public IActionResult LoginSSO()
        {
            // TODO idp 有待抽象化
            StateInfo stateInfo = new StateInfo
            {
                idp = 2,
                type = "Login"
            };
            string SSOUrl = Utility.GetGoogleSSOUrl(stateInfo);
            return StatusCode((int)HttpStatusCode.OK, new SuccessResponse { redirect_Uri = SSOUrl });
        }

        [HttpGet]
        [Route("/api/v1/checkPortalSSO")]
        public async Task<IActionResult> CheckProtalSSO()
        {
            var code = Request.Query["code"];
            Console.WriteLine("code: " + code);
            string rawState = Request.Query["state"];

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(rawState)) return StatusCode((int)HttpStatusCode.BadRequest, new
            {
                message = "query parameter wrong."
            });

            // TODO 可以移到env
            string client_id = "536062935773-e1hvscne4ead0kk62fho999kc179rhhj.apps.googleusercontent.com";
            string client_secret = "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";
            // string redirect_Uri = "https://localhost:5001/api/v1/checkPortalSSO";
            string redirect_Uri = "https://localhost:5001/";

            // get token
            var token = await new Utility().GetTokenFromCode(code, client_id, client_secret, redirect_Uri);
            Console.WriteLine(JsonConvert.SerializeObject(token, Formatting.Indented));
            // get user info
            var userInfo = await new Utility().GetUserInfo(token.access_token);
            Console.WriteLine(JsonConvert.SerializeObject(userInfo, Formatting.Indented)); // display
            try
            {
                string state = rawState.Replace("\\\"", "\"");
                // Console.WriteLine(state);
                StateInfo stateInfo = JsonConvert.DeserializeObject<StateInfo>(state);

                if (stateInfo.type == "Singup")
                {
                    // TODO bindingRelation idp 有待抽象化
                    sso_account_binding bindingRelationShip = new sso_account_binding
                    {
                        idp = stateInfo.idp,
                        sourceId = userInfo.id,
                        accountId = stateInfo.accountId,
                    };
                    (bool isSuccess, string message) = this._ssoAccountData.BindAccount(bindingRelationShip);
                    int statusCode = isSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
                    return StatusCode(statusCode, new { message = message });
                }
                else if (stateInfo.type == "Login")
                {
                    (bool isSuccess, string message) = this._ssoAccountData.LoginSSOAccount(stateInfo, userInfo.id);
                    int httpStatus = isSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.BadRequest;
                    return StatusCode(httpStatus, new { message = message });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("state formate in not json");
                return StatusCode((int)HttpStatusCode.BadRequest, new FailedResponse { message = "state formate is not json" });
            }

            return StatusCode((int)HttpStatusCode.OK);
        }
    }
}