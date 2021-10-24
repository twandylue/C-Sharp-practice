using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Respository
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {

        }

        public DbSet<AccountModel> Accounts {get; set;}
    }
}