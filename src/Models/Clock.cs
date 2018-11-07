using System;
using AlarmClock.Misc;

namespace AlarmClock.Models
{
    public class Clock : NotifyPropertyChanged
    {
        public  Guid Id { get; }
        private DateTime _lastTriggered;
        private DateTime _nextTrigger;
        private User     _owner;

        #region properites
        public DateTime LastTriggered
        {
            get => _lastTriggered;
            set
            {
                _lastTriggered = value;
                OnPropertyChanged(nameof(LastTriggered));
            }
        }

        public DateTime NextTrigger
        {
            get => _nextTrigger;
            set
            {
                _nextTrigger = value;
                OnPropertyChanged(nameof(NextTrigger));
            }
        }

        public User Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
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
