using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Respository
{
    public class PostgresDBContext : DbContext
    {
        public PostgresDBContext(DbContextOptions<PostgresDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(c => c.Platform!)
                .HasForeignKey(c => c.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(c => c.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
                .Entity<Account>()
                .HasIndex(p => p.AccountName)
                .IsUnique();

            modelBuilder
                .Entity<Account>()
                .HasMany(p => p.bindings)
                .WithOne(s => s.account)
                .HasForeignKey(s => s.accountId);

            // * 與上句等效
            // modelBuilder
            //     .Entity<sso_account_binding>()
            //     .HasOne(s => s.account)
            //     .WithMany( a => a.bindings)
            //     .HasForeignKey(s => s.accountId);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<GoogleUserInfo> GoogleUserInfos {get; set;}
        public DbSet<sso_account_binding> sso_account_binding {get; set;}
    }
}