using System;
using System.Data.Entity.ModelConfiguration;

namespace AlarmClock.DBModels
{
    [Serializable]
    public class Clock
    {
        #region properites
        public  Guid Id { get; private set; }

        public DateTime LastTriggered { get; set; }

        public DateTime NextTrigger { get; set; }

        public User Owner { get; private set; }

        public Guid UserId { get; private set; }
        #endregion

        #region constructors
        public Clock(DateTime lastTriggered, DateTime nextTrigger, User owner)
        {
            Id = Guid.NewGuid();

            LastTriggered = lastTriggered;
            NextTrigger = nextTrigger;
            Owner = owner;
            UserId = Owner.Id;
        }

        private Clock()
        {
        }

        #endregion

        #region EntityFrameworkConfiguration
        public class ClockEntityConfiguration : EntityTypeConfiguration<Clock>
        {
            public ClockEntityConfiguration()
            {
                ToTable("Clock");
                HasKey(s => s.Id);

                Property(p => p.Id)
                    .HasColumnName("Id")
                    .IsRequired();
                Property(p => p.LastTriggered)
                    .HasColumnName("LastTriggered")
                    .IsRequired();
                Property(s => s.NextTrigger)
                    .HasColumnName("NextTrigger")
                    .IsRequired();
            }
        }
        #endregion
        public void DeleteDatabaseValues()
        {
            Owner = null;
        }
    }
}