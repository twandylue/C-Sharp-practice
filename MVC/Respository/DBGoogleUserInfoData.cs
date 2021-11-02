using System;
using System.Linq;
using System.Collections.Generic;
using MVC.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MVC.Respository
{
    public class DBGoogleUserInfoData : IUserInfoData
    {
        private PostgresDBContext _DbContext;
        private readonly ILogger<DBGoogleUserInfoData> _logger;
        public DBGoogleUserInfoData(PostgresDBContext context, ILogger<DBGoogleUserInfoData> logger)
        {
            _DbContext = context;
            _logger = logger;
        }
        public List<GoogleUserInfo> GetUserInfos()
        {
            return _DbContext.GoogleUserInfos.ToList<GoogleUserInfo>();
        }
        // public GoogleUserInfo GetUserInfo(string accountName)
        // {
        //     _logger.LogInformation("account name: " + accountName);
        //     // var ret = from a in _DbContext.GoogleUserInfos where a.accountName == accountName select a;
        //     var ret = this._DbContext.GoogleUserInfos.Where(a => a.accountName == accountName).AsNoTracking().ToList<GoogleUserInfo>();
        //     _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
        //     if (ret.Count == 0) return null;
        //     return ret[0];
        // }
        public GoogleUserInfo AddUserInfo(string accountName, GoogleUserInfo googleUserInfo)
        {
            this._DbContext.GoogleUserInfos.Add(googleUserInfo);
            this._DbContext.SaveChangesAsync();
            return googleUserInfo;
        }
    }
}