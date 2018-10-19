using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using AlarmClock.Annotations;

namespace AlarmClock.Models
{
    public class Clock : INotifyPropertyChanged
    {
        private string _id;
        private DateTime _lastTriggered;
        private DateTime _nextTrigger;
        private User     _owner;

        #region properites
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
