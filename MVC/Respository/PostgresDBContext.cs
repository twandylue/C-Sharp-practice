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
                .WithOne(p => p.Platform!)
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<GoogleUserInfo> GoogleUserInfos {get; set;}
        
    }
}