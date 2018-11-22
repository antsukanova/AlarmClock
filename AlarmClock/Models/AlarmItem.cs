using AlarmClock.DBModels;
using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
using AlarmClock.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AlarmClock.Models
{
    public class AlarmItem : NotifyPropertyChanged
    {
        #region attributes
        private enum TimeProp
        {
            Hour,
            Minute
        }

        private static readonly Regex Regex = new Regex("[^0-9.-]+");
        private const byte MaxMinutes = (byte) (TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond) - 1;
        private const byte MaxHours = (byte) (TimeSpan.TicksPerDay / TimeSpan.TicksPerHour) - 1;

        private readonly ObservableCollection<AlarmItem> _owner;

        private int _hour;
        private int _minute;
        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;
        private ICommand _addAlarm;
        private ICommand _deleteAlarm;
        private ICommand _ringAlarm;

        private bool _isActive;
        private bool _isStopped;

        #endregion

        #region properties
        public Clock Clock { get; private set; }

        public List<AlarmItem> UserAlarms => _owner
                                                .Where(item => !item.IsBaseAlarm)
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
            }
        }

        public bool IsBaseAlarm => Clock == null;
        public bool IsAddEnabled => IsBaseAlarm;
        public bool IsRingEnabled => !IsBaseAlarm;
        public bool IsDeleteEnabled => !IsBaseAlarm;

        private bool CanAddAlarmExecute(object obj) =>
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

        public bool IsStopped
        {
            get => _isStopped;
            set
            {
                _isStopped = value;

                IsActive = !IsActive;
            }
        }

        public bool IsEnabled => IsBaseAlarm || !IsActive;
        #endregion

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand<object>(
                delegate { ChangeAlarm(ref _hour, TimeProp.Hour, 1, MaxHours); }
            ));

        public ICommand ClickDownHour =>
            _clickDownHour ??
           (_clickDownHour = new RelayCommand<object>(
                delegate { ChangeAlarm(ref _hour, TimeProp.Hour, -1, MaxHours); }
           ));

        public ICommand ClickUpMinute =>
            _clickUpMinute ??
           (_clickUpMinute = new RelayCommand<object>(
               delegate { ChangeAlarm(ref _minute, TimeProp.Minute, 1, MaxMinutes); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ??
           (_clickDownMinute = new RelayCommand<object>(
                delegate { ChangeAlarm(ref _minute, TimeProp.Minute, -1, MaxMinutes); }
           ));

        public ICommand AddAlarm => _addAlarm ?? (_addAlarm = new RelayCommand<object>(AddAlarmExecute, CanAddAlarmExecute));

        public ICommand DeleteAlarm => _deleteAlarm ?? (_deleteAlarm = new RelayCommand<object>(DeleteAlarmExecute));

        public ICommand RingAlarm => _ringAlarm ?? (_ringAlarm = new RelayCommand<object>(
            delegate
            {
                if (IsActive)
                    _isStopped = true;
                IsActive = !IsActive;
            }));

        #endregion

        public AlarmItem(ObservableCollection<AlarmItem> owner, int hour, int minute)
        {
            _owner = owner;
            _hour = hour;
            _minute = minute;
        }

        private static int GetTimeValue(AlarmItem ai) => ai._hour * (MaxMinutes + 1) + ai._minute;
        private static int GetTimeValue(DateTime dt) => dt.Hour * (MaxMinutes +1 ) + dt.Minute;

        public bool Equals(DateTime dt) => GetTimeValue(dt) == GetTimeValue(this);

        private void DeleteAlarmExecute(object obj)
        {
            DbManager.DeleteClock(Clock);
            _owner.Remove(this);

            Logger.Log($"Alarm clock {Clock.Id} was deleted.");
        }

        private void ChangeAlarm(ref int propValue, TimeProp property, int offset, byte highBound)
        {
            var newValue = propValue + offset;

            propValue = newValue == -1 ? highBound : (newValue == highBound + 1 ? 0 : newValue);

            _isStopped = false;

            OnPropertyChanged(property.ToString());

            if (IsBaseAlarm)
                return;

            switch (property)
            {
                case TimeProp.Hour:
                    Clock.NextTrigger = GetNewClockTime(newValue, _minute);
                    break;
                case TimeProp.Minute:
                    Clock.NextTrigger = GetNewClockTime(_hour, newValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(property), property, @"Unknown TimeProp.");
            }

            Logger.Log($"Property {property.ToString()} of Alarm clock {Clock.Id} was updated to have value {propValue}.");
        }

        private void AddAlarmExecute(object obj)
        {
            Logger.Log("Trying to add new Alarm Clock.");

            try
            {
                var newTime = GetNewClockTime(_hour, _minute);
                var clock = DbManager.AddClock(
                    new Clock(newTime, newTime.AddDays(1), StationManager.CurrentUser)
                );

                AddClockToCollection(clock);

                Logger.Log($"Alarm clock {clock.Id} with time {clock.NextTrigger} was successfully added" +
                           $" by the User {StationManager.CurrentUser.Id}.");
            }
            catch (Exception e)
            {
                MessageBox.Show(Resources.CantParseTimeError);
                Logger.Log(e, Resources.CantParseTimeError);
            }
        }

        public void AddClockToCollection(Clock clock)
        {
            var alarm = new AlarmItem(
                    _owner, clock.NextTrigger.Hour, clock.NextTrigger.Minute
                )
                { Clock = clock };

            var index = UserAlarms.FindIndex(item => GetTimeValue(item) > GetTimeValue(alarm));

            _owner.Insert(index == -1 ? _owner.Count : index + 1, alarm);
        }

        private static DateTime GetNewClockTime(int hour, int minute)
        {
            var tmpDate = DateTime.Now;
            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        private static bool IsValidTime(string time, int max) =>
            !Regex.IsMatch(time) && time.Length == 2 &&
                int.Parse(time) >= 0 && int.Parse(time) <= max;

        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            var myCaretIndex = ((TextBox)e.Source).CaretIndex;
            var characters = ((TextBox)e.Source).Text.ToCharArray();

            if (myCaretIndex >= characters.Length)
                return;

            characters[myCaretIndex] = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            ((TextBox)e.Source).Text = string.Join("", characters);
            ((TextBox)e.Source).CaretIndex = myCaretIndex + 1;

            e.Handled = true;
        }
    }
}
