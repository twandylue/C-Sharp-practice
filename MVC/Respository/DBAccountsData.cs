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
        private AccountContext _accountContext;
        private readonly ILogger<DBAccountsData> _logger;
        public DBAccountsData(AccountContext accountContext, ILogger<DBAccountsData> logger)
        {
            _accountContext = accountContext;
            _logger = logger;
        }
        public List<AccountModel> GetAccounts()
        {
            return this._accountContext.Accounts.ToList();
        }
        public AccountModel GetAccount(String accountName)
        {
            _logger.LogInformation("target: " + accountName);
            // var ret = from a in _accountContext.Accounts where a.AccountName == accountName select a; // linq
            var ret = this._accountContext.Accounts.Where(a => a.AccountName == accountName).AsNoTracking().ToList(); // arrow func
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
            this._accountContext.Accounts.Add(account);
            this._accountContext.SaveChanges();
            return account;
        }
        public void DeleteAccount(AccountModel account)
        {
            this._accountContext.ChangeTracker.DetectChanges();
            _logger.LogInformation(this._accountContext.ChangeTracker.DebugView.LongView);
            // this._accountContext.Accounts.AsNoTracking();
            this._accountContext.Accounts.Remove(account);
            this._accountContext.SaveChanges();
        }
        // public AccountModel EditAccount(AccountModel account) {
        //     var ExistingAccount = this._accountContext.Accounts.Find(account.AccountName); // 待改
        //     if (ExistingAccount != null) {
        //         ExistingAccount.AccountName = 
        //     }
        // }
    }
}