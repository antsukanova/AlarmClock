using System;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

namespace AlarmClock.DBModels
{
    [Serializable]
    [DataContract(IsReference = true)]
    public class Clock
    {
        #region properites
        [DataMember] public Guid Id { get; private set; }
        [DataMember] public DateTime LastTriggered { get; set; }
        [DataMember] public DateTime NextTrigger { get; set; }
        [DataMember] public User Owner { get; private set; }
        [DataMember] public Guid UserId { get; private set; }
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

                Property(p => p.Id).HasColumnName("Id").IsRequired();
                Property(p => p.LastTriggered).HasColumnName("LastTriggered").IsRequired();
                Property(s => s.NextTrigger).HasColumnName("NextTrigger").IsRequired();
            }
        }
        #endregion

        public void ClearReferences() => Owner = null;
    }
}