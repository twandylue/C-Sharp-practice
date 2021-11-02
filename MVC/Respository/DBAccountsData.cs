using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MVC.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MVC.Respository
{
    public class DBAccountsData : IAccountData
    {
        private PostgresDBContext _appDBContext;
        private readonly ILogger<DBAccountsData> _logger;
        public DBAccountsData(PostgresDBContext context, ILogger<DBAccountsData> logger)
        {
            _appDBContext = context;
            _logger = logger;
        }
        public List<Account> GetAccounts()
        {
            return this._appDBContext.Accounts.ToList();
        }
        public Account GetAccount(String accountName)
        {
            _logger.LogInformation("target: " + accountName);
            // var ret = from a in _appDBContext.Accounts where a.AccountName == accountName select a; // linq
            var ret = this._appDBContext.Accounts.Where(a => a.AccountName == accountName).AsNoTracking().ToList(); // lamda func
            _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
            var resAccount = new Account();
            foreach (Account account in ret)
            {
                resAccount.Id = account.Id;
                resAccount.AccountName = account.AccountName;
                resAccount.Password = account.Password;
            }
            return resAccount;

            /* 另類寫法 但會出問題，當找不到符合條件的資料時，會有問題
            _logger.LogInformation("target: " + accountName);
            var ret = this._appDBContext.Accounts.Where(a => a.AccountName == accountName).AsNoTracking().ToList<Account>(); // arrow func
            _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
            if (ret.Count == 0) _logger.LogInformation("test------");
            return ret[0];
            */
        }
        public Account AddAccount(Account account)
        {
            this._appDBContext.Accounts.Add(account);
            this._appDBContext.SaveChanges();
            return account;
        }
        public void DeleteAccount(Account account)
        {
            this._appDBContext.ChangeTracker.DetectChanges();
            _logger.LogInformation(this._appDBContext.ChangeTracker.DebugView.LongView);
            this._appDBContext.Accounts.Remove(account);
            this._appDBContext.SaveChanges();
        }
        // public Account EditAccount(Account account) {
        //     var ExistingAccount = this._appDBContext.Accounts.Find(account.AccountName); // 待改
        //     if (ExistingAccount != null) {
        //         ExistingAccount.AccountName = 
        //     }
        // }
    }
}