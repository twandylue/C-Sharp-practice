using System;
using System.Collections.Generic;
using MVC.Models;

namespace MVC.Respository
{
    public interface IUserInfoData
    {
        List<GoogleUserInfo> GetUserInfos ();
        GoogleUserInfo GetUserInfo (string accountName);
        GoogleUserInfo AddUserInfo (string accountName, GoogleUserInfo googleUserInfo);
        // void DeleteGoogleUserInfo(Account account);
    }
}