using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace MVC.Respository
{
    public class DBSSOAccountData : ISSOAccount
    {
        private readonly IDbContextFactory<PostgresDBContext> _dbContextFactory;
        private readonly ILogger<DBSSOAccountData> _logger;
        public DBSSOAccountData(IDbContextFactory<PostgresDBContext> dbContextFactory, ILogger<DBSSOAccountData> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }
        public (bool, string) BindAccount(sso_account_binding bindingRelationShip)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                var ret = dbCtx.sso_account_binding.Where(s => s.sourceId == bindingRelationShip.sourceId && s.idp == bindingRelationShip.idp).ToList();
                if (ret.Count != 0) return (false, "sso id is already used");
                dbCtx.sso_account_binding.Add(bindingRelationShip);
                dbCtx.SaveChanges();
                return (true, "binding sso id success");
            }
        }
        public (bool, string) LoginSSOAccount(StateInfo stateInfo, string sourceId)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                var ret = dbCtx.sso_account_binding.Where(s => s.sourceId == sourceId && s.idp == stateInfo.idp).ToList();
                _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
                if (ret.Count == 0) return (false, "Haven't Singup");

                var accountInfos = dbCtx.sso_account_binding
                    .Where(
                        s => s.sourceId == sourceId &&
                        s.idp == stateInfo.idp
                    )
                    .Join(
                        dbCtx.Accounts,
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
    }

    public class accountInfo
    {
        public int _accountId { get; set; }
        public string _accountName { get; set; }
    }
}