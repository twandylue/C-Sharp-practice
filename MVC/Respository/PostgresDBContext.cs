using Microsoft.EntityFrameworkCore;
using MVC.GraphQL.Data;
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
            modelBuilder.Entity<Attendee>().HasIndex(a => a.UserName).IsUnique();
            // Many-to-many: Session <-> Attendee
            modelBuilder
                .Entity<SessionAttendee>()
                .HasKey(ca => new { ca.SessionId, ca.AttendeeId });

            // Many-to-many: Speaker <-> Session
            modelBuilder
                .Entity<SessionSpeaker>()
                .HasKey(ss => new { ss.SessionId, ss.SpeakerId });

        }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<Session> Sessions { get; set; } = default!;

        public DbSet<Track> Tracks { get; set; } = default!;

        public DbSet<Speaker> Speakers { get; set; } = default!;

        public DbSet<Attendee> Attendees { get; set; } = default!;
    }
}