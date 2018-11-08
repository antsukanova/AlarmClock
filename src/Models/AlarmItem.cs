using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
using AlarmClock.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AlarmClock.Models
{
    public class AlarmItem : NotifyPropertyChanged
    {
        #region attributes
        private static readonly Regex Regex = new Regex("[^0-9.-]+");
        private const byte MaxMinutes = (byte) (TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond) - 1;
        private const byte MaxHours = (byte) (TimeSpan.TicksPerDay / TimeSpan.TicksPerHour) - 1;

        private readonly ObservableCollection<AlarmItem> _owner;
        private readonly ClockRepository _clocks;

        private int _hour;
        private int _minute;
        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;
        private ICommand _addAlarm;
        private ICommand _deleteAlarm;
        private ICommand _bellAlarm;

        private bool _isActive;
        private bool _isVisible = true;

        #endregion

        #region properties
        public Clock Clock { get; private set; }

        public List<AlarmItem> UserAlarms => _owner
            .Where(item => !item.IsBaseAlarm && item.IsVisible)
            .ToList();

        public string Hour
        {
            get => $"{_hour:00}";
            set
            {
                if (!IsValidTime(value, MaxHours))
                    return;

                _hour = int.Parse(value);

                OnPropertyChanged(nameof(Hour));
                OnPropertyChanged(nameof(IsAllowedTime));
            }
        }

        public string Minute
        {
            get => $"{_minute:00}";
            set
            {
                if (!IsValidTime(value, MaxMinutes))
                    return;

                _minute = int.Parse(value);

                OnPropertyChanged(nameof(Minute));
                OnPropertyChanged(nameof(IsAllowedTime));
            }
        }

        public bool IsBaseAlarm => Clock == null;
        public bool IsAddEnabled => IsBaseAlarm;
        public bool IsSaveEnabled => !IsBaseAlarm;
        public bool IsCancelEnabled => !IsBaseAlarm;
        public bool IsBellEnabled => !IsBaseAlarm;
        public bool IsDeleteEnabled => !IsBaseAlarm;

        public bool IsAllowedTime =>
            !UserAlarms
                   .Where(item => item != this)
                   .Select(GetTimeValue)
                   .Contains(GetTimeValue(this));

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if ((!value || UserAlarms.Any(item => item.IsActive)) && value)
                    return;

                _isActive = value;

                OnPropertyChanged(nameof(IsEnabled));
                OnPropertyChanged(nameof(IsActive));

                Logger.Log($"Alarm clock {Clock.Id} started ringing.");
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;

                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public bool IsStopped { get; private set; }
        public bool IsEnabled => IsBaseAlarm || !IsActive;
        #endregion

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, TimeProp.Hour, 1, MaxHours); }
            ));

        public ICommand ClickDownHour =>
            _clickDownHour ??
           (_clickDownHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, TimeProp.Hour, -1, MaxHours); }
           ));

        public ICommand ClickUpMinute =>
            _clickUpMinute ??
           (_clickUpMinute = new RelayCommand(
               delegate { ChangeAlarm(ref _minute, TimeProp.Minute, 1, MaxMinutes); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ??
           (_clickDownMinute = new RelayCommand(
                delegate { ChangeAlarm(ref _minute, TimeProp.Minute, -1, MaxMinutes); }
           ));

        public ICommand AddAlarm => _addAlarm ?? (_addAlarm = new RelayCommand(AddAlarmExecute));

        public ICommand DeleteAlarm => _deleteAlarm ?? (_deleteAlarm = new RelayCommand(DeleteAlarmExecute));

        public ICommand BellAlarm => _bellAlarm ?? (_bellAlarm = new RelayCommand(
            delegate
            {
                if (IsActive)
                    IsStopped = true;
                IsActive = !IsActive;
            }));
        #endregion

        public AlarmItem(ObservableCollection<AlarmItem> owner, ClockRepository clocks, int hour, int minute)
        {
            _owner = owner;
            _clocks = clocks;
            _hour = hour;
            _minute = minute;
        }

        private static int GetTimeValue(AlarmItem ai) => ai._hour * MaxMinutes + ai._minute;
        private static int GetTimeValue(DateTime dt) => dt.Hour * MaxMinutes + dt.Minute;

        public bool Equals(DateTime dt) => GetTimeValue(dt) == GetTimeValue(this);

        private void DeleteAlarmExecute(object obj)
        {
            var userClocks = _clocks.ForUser(StationManager.CurrentUser.Id);

            for (int i = 1, j = 0; i < _owner.Count; i++)
            {
                if (!_owner[i].IsVisible || _owner[i] == this)
                    continue;

                _owner[i].Clock.LastTriggered = j == 0 ? new DateTime() : userClocks[j - 1].NextTrigger;

                _owner[i].Clock.NextTrigger = j + 1 == userClocks.Count ? new DateTime() : userClocks[j + 1].LastTriggered;

                // (in process for filter clocks in the same order as alarms)_clocks[i - 1] = _owner[i].Clock;
                j++;
            }

            _clocks.Delete(Clock.Id);
            Logger.Log($"Alarm clock {Clock.Id} was deleted.");

            _owner.Remove(this);

            Update();
        }

        private void ChangeAlarm(ref int propValue, TimeProp property, int offset, byte highBound)
        {
            var newValue = propValue + offset;

            propValue = newValue == -1 ? highBound : (newValue == highBound + 1 ? 0 : newValue);

            IsStopped = false;

            OnPropertyChanged(property.ToString());
            OnPropertyChanged(nameof(IsAllowedTime));

            Update();

            if (IsBaseAlarm)
                return;

            switch (property)
            {
                case TimeProp.Hour:
                    Clock.NextTrigger = GetNewClockTime(newValue, _minute); // maybe should be replaced by Update() call
                    break;
                case TimeProp.Minute:
                    Clock.NextTrigger = GetNewClockTime(_hour, newValue); // maybe should be replaced by Update() call
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(property), property, "Unknown TimeProp.");
            }

            Logger.Log($"Property {property.ToString()} of Alarm clock {Clock.Id} was updated to have value {propValue}.");
        }

        public void Update()
        {
            var now = DateTime.Now;

            //TODO change time in first alarm 
            //_hour = now.Hour;
            //_minute = now.Minute;
            _owner[0].OnPropertyChanged(nameof(IsAllowedTime));
            //OnPropertyChanged(nameof(Hour));
            //OnPropertyChanged(nameof(Minute));
        } 

        private void AddAlarmExecute(object obj)
        {
            Logger.Log("Trying to add new Alarm Clock.");

            try
            {
                var clock = new Clock(
                    GetNewClockTime(_hour, _minute), DateTime.Now, StationManager.CurrentUser
                );

                _owner.Add(new AlarmItem(_owner, _clocks, _hour, _minute)
                {
                    Clock = _clocks.Add(clock)
                });

                Logger.Log($"Alarm clock {clock.Id} with time {clock.NextTrigger} was successfully added" +
                           $" by the User {StationManager.CurrentUser.Id}.");

                Rearrange();

                OnPropertyChanged(nameof(IsAllowedTime));
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.CantParseTimeError);
                Logger.Log(ex, Resources.CantParseTimeError);
                return;
            }
        }

        public void Rearrange()
        {
            var sortedList = UserAlarms.OrderBy(GetTimeValue).ToList();

            for (int i = 1, j = 0; i < _owner.Count; i++)
            {
                if (!_owner[i].IsVisible)
                    continue;

                _owner[i] = sortedList[j];

                if (j > 0)
                    _owner[i].Clock.LastTriggered = GetNewClockTime(sortedList[j - 1]._hour, sortedList[j - 1]._minute);

                if (j < sortedList.Count - 1)
                    _owner[i].Clock.NextTrigger = GetNewClockTime(sortedList[j + 1]._hour, sortedList[j + 1]._minute);

                //(the same as above)_clocks[i - 1] = _owner[i].Clock;
                j++;
            }
        }

        private static DateTime GetNewClockTime(int hour, int minute)
        {
            var tmpDate = DateTime.Now;

            // check if clock will NOT ring today
            if (TimeSpan.Compare(tmpDate.TimeOfDay, new TimeSpan(hour, minute, tmpDate.Second)) >= 0)
                tmpDate = tmpDate.AddDays(1);

            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        private static bool IsValidTime(string time, int max) =>
            !Regex.IsMatch(time) && time.Length == 2 &&
                int.Parse(time) >= 0 && int.Parse(time) <= max;
    }

    enum TimeProp
    {
        Hour,
        Minute
    }
}
