using System;
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
        public sso_account_binding BindAccount(sso_account_binding bindingRelationShip)
        {
            this._appDbContext.sso_account_binding.Add(bindingRelationShip);
            this._appDbContext.SaveChanges();
            return bindingRelationShip;
        }
        public (bool, string) LoginSSOAccount(StateInfo stateInfo, string sourceId)
        {
            var ret = this._appDbContext.sso_account_binding.Where(s => s.sourceId == sourceId && s.idp == stateInfo.idp ).ToList();
            _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
            var testJoin = this._appDbContext.sso_account_binding
                .Join(
                    this._appDbContext.Accounts,
                    bindInfo => bindInfo.accountId,
                    account => account.Id,
                    (bindInfo, account) => new loginInfo {
                        // _bindinfo = bindInfo,
                        _account = account
                    }
                ).ToList();
            // TODO inner join and outter join 待補 有誤
            // todo 轉跳頁面
            _logger.LogInformation(JsonConvert.SerializeObject(testJoin[0], Formatting.Indented));
            // foreach (var item in testJoin) _logger.LogInformation(JsonConvert.SerializeObject(item));

            if (ret.Count == 0) return (false, "Haven't Singup");
            return (true, "You get a token...");
        }
    }

    public class loginInfo {
        public sso_account_binding _bindinfo {get; set;}
        public Account _account { get; set; }
    }
}