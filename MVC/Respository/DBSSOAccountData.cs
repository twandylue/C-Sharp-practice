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
        public (bool, string) LoginSSOAccount(string sourceId)
        {
            var ret = this._appDbContext.sso_account_binding.Where(s => s.sourceId == sourceId).ToList();
            _logger.LogInformation(JsonConvert.SerializeObject(ret, Formatting.Indented));
            if (ret.Count == 0) return (false, "Haven't Singup");
            return (true, "You get a token...");
        }
    }
}