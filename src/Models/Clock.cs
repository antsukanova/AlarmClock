using System;

namespace AlarmClock.Models
{
    [Serializable]
    public class Clock
    {
        #region properites
        public  Guid Id { get; }

        public DateTime LastTriggered { get; set; }

        public DateTime NextTrigger { get; set; }

        public User Owner { get; }
        #endregion

        #region constructors
        public Clock(DateTime lastTriggered, DateTime nextTrigger, User owner)
        {
            Id = Guid.NewGuid();

            LastTriggered = lastTriggered;
            NextTrigger = nextTrigger;
            Owner = owner;
        }
        #endregion
    }
}
