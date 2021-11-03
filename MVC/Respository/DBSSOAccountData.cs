using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Respository
{
    public class DBSSOAccountData : ISSOAccount
    {
        private PostgresDBContext _appDbContext;
        private readonly ILogger<DBSSOAccountData> _logger;
        public DBSSOAccountData(PostgresDBContext context, ILogger<DBSSOAccountData> logger)
        {
            _appDbContext = context;
            _logger = logger;
        }
        public (bool, string) BindAccount(sso_account_binding bindingRelationShip)
        {
            var ret = this._appDbContext.sso_account_binding.Where(s => s.sourceId == bindingRelationShip.sourceId && s.idp == bindingRelationShip.idp).ToList();
            if (ret.Count != 0) return (false, "sso id is already used");
            this._appDbContext.sso_account_binding.Add(bindingRelationShip);
            this._appDbContext.SaveChanges();
            return (true, "binding sso id success");
        }
        public (bool, string) LoginSSOAccount(StateInfo stateInfo, string sourceId)
        {
            var ret = this._appDbContext.sso_account_binding.Where(s => s.sourceId == sourceId && s.idp == stateInfo.idp).ToList();
            _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
            if (ret.Count == 0) return (false, "Haven't Singup");

            var accountInfos = this._appDbContext.sso_account_binding
                .Where(
                    s => s.sourceId == sourceId &&
                    s.idp == stateInfo.idp
                )
                .Join(
                    this._appDbContext.Accounts,
                    binding => binding.accountId,
                    account => account.Id,
                    (binding, account) => new accountInfo
                    {
                        _accountId = account.Id,
                        _accountName = account.AccountName
                    }
                ).ToList()[0];

            _logger.LogInformation(JsonConvert.SerializeObject(accountInfos, Formatting.Indented));

            return (true, $"Hi! {accountInfos._accountName}. Welcome back! (get a Portal token...)");
        }
    }

    public class accountInfo
    {
        public int _accountId { get; set; }
        public string _accountName { get; set; }
    }
}