using System;
using System.Collections.Generic;
using MVC.Models;

namespace MVC.Respository
{
    public interface IAccountData
    {
        List<AccountModel> GetAccounts();
        AccountModel GetAccount(String accountName);
        AccountModel AddAccount(AccountModel account);
        void DeleteAccount(AccountModel account);
        // AccountModel EditAccount(AccountModel account);
    }
}