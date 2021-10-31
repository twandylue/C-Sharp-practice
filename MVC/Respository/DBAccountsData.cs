using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MVC.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
        public List<AccountModel> GetAccounts()
        {
            return this._appDBContext.Accounts.ToList();
        }
        public AccountModel GetAccount(String accountName)
        {
            _logger.LogInformation("target: " + accountName);
            // var ret = from a in _appDBContext.Accounts where a.AccountName == accountName select a; // linq
            var ret = this._appDBContext.Accounts.Where(a => a.AccountName == accountName).AsNoTracking().ToList(); // arrow func
            _logger.LogInformation(JsonSerializer.Serialize(ret));
            var resAccount = new AccountModel();
            foreach (AccountModel account in ret)
            {
                resAccount.Id = account.Id;
                resAccount.AccountName = account.AccountName;
                resAccount.Password = account.Password;
            }
            return resAccount;
        }
        public AccountModel AddAccount(AccountModel account)
        {
            this._appDBContext.Accounts.Add(account);
            this._appDBContext.SaveChanges();
            return account;
        }
        public void DeleteAccount(AccountModel account)
        {
            this._appDBContext.ChangeTracker.DetectChanges();
            _logger.LogInformation(this._appDBContext.ChangeTracker.DebugView.LongView);
            this._appDBContext.Accounts.Remove(account);
            this._appDBContext.SaveChanges();
        }
        // public AccountModel EditAccount(AccountModel account) {
        //     var ExistingAccount = this._appDBContext.Accounts.Find(account.AccountName); // 待改
        //     if (ExistingAccount != null) {
        //         ExistingAccount.AccountName = 
        //     }
        // }
    }
}