using System;
using System.Collections.Generic;
using MVC.Models;

namespace MVC.Respository
{
    public interface IAccountData
    {
        List<Account> GetAccounts();
        Account GetAccount(String accountName);
        Account AddAccount(Account account);
        (bool, Account) CheckAccount (Account account);
        void DeleteAccount(Account account);
    }
}