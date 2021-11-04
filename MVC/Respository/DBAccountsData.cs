using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MVC.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MVC.Respository
{
    public class DBAccountsData : IAccountData
    {
        private readonly IDbContextFactory<PostgresDBContext> _dbContextFactory;
        private readonly ILogger<DBAccountsData> _logger;
        public DBAccountsData(IDbContextFactory<PostgresDBContext> dbContextFactory, ILogger<DBAccountsData> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }
        public List<Account> GetAccounts()
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                return dbCtx.Accounts.ToList();
            }
        }
        public Account GetAccount(String accountName)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                _logger.LogInformation("target: " + accountName);
                // var ret = from a in dbCtx.Accounts where a.AccountName == accountName select a; // linq
                var ret = dbCtx.Accounts.Where(a => a.AccountName == accountName).AsNoTracking().ToList(); // lamda func
                _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
                var resAccount = new Account();
                foreach (Account account in ret)
                {
                    resAccount.Id = account.Id;
                    resAccount.AccountName = account.AccountName;
                    resAccount.Password = account.Password;
                }
                return resAccount;
            }
        }
        public Account AddAccount(Account account)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                dbCtx.Accounts.Add(account);
                dbCtx.SaveChanges();
                return account;
            }
        }
        public void DeleteAccount(Account account)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                dbCtx.ChangeTracker.DetectChanges();
                _logger.LogInformation(dbCtx.ChangeTracker.DebugView.LongView);
                dbCtx.Accounts.Remove(account);
                dbCtx.SaveChanges();
            }
        }
        public (bool, Account) CheckAccount(Account account)
        {
            using (var dbCtx = this._dbContextFactory.CreateDbContext())
            {
                var ret = dbCtx.Accounts.Where(
                    ac => ac.AccountName == account.AccountName &&
                    ac.Password == account.Password
                ).ToList();
                if (ret.Count == 0) return (false, null);
                return (true, ret[0]);
            }
        }
    }
}