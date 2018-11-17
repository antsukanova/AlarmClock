using System.Data.Entity;
using AlarmClock.DBAdapter.Migrations;
using AlarmClock.DBModels;

namespace AlarmClock.DBAdapter
{
    internal class ClockDBContext : DbContext
    {
        public ClockDBContext() : base("NewClockDB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ClockDBContext, Configuration>(true));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Clock> Clocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new User.UserEntityConfiguration());
            modelBuilder.Configurations.Add(new Clock.ClockEntityConfiguration());
        }
    }
}
