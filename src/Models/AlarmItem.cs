using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
using AlarmClock.Repositories;
using System;
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
        private const byte MaxMinutes = (byte) (TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond) - 1;
        private const byte MaxHours = (byte) (TimeSpan.TicksPerDay / TimeSpan.TicksPerHour) - 1;

        private static readonly Regex Regex = new Regex("[^0-9.-]+");
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

        private bool _isBlinking;
        #endregion

        #region properties
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

        public bool IsBaseAlarm => _owner.IndexOf(this) == 0;
        public bool IsAddEnabled => IsBaseAlarm;
        public bool IsSaveEnabled => !IsBaseAlarm;
        public bool IsCancelEnabled => !IsBaseAlarm;
        public bool IsBellEnabled => !IsBaseAlarm;
        public bool IsDeleteEnabled => !IsBaseAlarm;

        public bool IsAllowedTime =>
            !_owner.Skip(1)
                   .Where(item => item != this)
                   .Select(GetTimeValue)
                   .Contains(GetTimeValue(this));

        public bool IsBlinking
        {
            get => _isBlinking;
            set
            {
                _isBlinking = value;
                OnPropertyChanged(nameof(IsBlinking));
            }
        }
        #endregion

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), 1, MaxHours); }
            ));

        public ICommand ClickDownHour =>
            _clickDownHour ??
           (_clickDownHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), -1, MaxHours); }
           ));

        public ICommand ClickUpMinute =>
            _clickUpMinute ??
           (_clickUpMinute = new RelayCommand(
               delegate { ChangeAlarm(ref _minute, nameof(Minute), 1, MaxMinutes); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ??
           (_clickDownMinute = new RelayCommand(
                delegate { ChangeAlarm(ref _minute, nameof(Minute), -1, MaxMinutes); }
           ));

        public ICommand AddAlarm => _addAlarm ?? (_addAlarm = new RelayCommand(AddAlarmClockExecute));

        public ICommand DeleteAlarm => _deleteAlarm ?? (_deleteAlarm = new RelayCommand(DoDeleteAlarm));

        public ICommand BellAlarm => _bellAlarm ?? (_bellAlarm = new RelayCommand(DoBellAlarm));
        #endregion

        public AlarmItem(ObservableCollection<AlarmItem> owner, ClockRepository clocks, int hour, int minute)
        {
            _owner = owner;
            _clocks = clocks;
            _hour = hour;
            _minute = minute;
        }

        private static int GetTimeValue(AlarmItem ai) => ai._hour * MaxMinutes + ai._minute;
        private void DoDeleteAlarm(object obj)
        {
            var currentIndex = GetAlarmIndex() - 1;

            _owner.Remove(this);

            var userClocks = _clocks.ForUser(StationManager.CurrentUser.Id);

            if (currentIndex > 0)
                userClocks[currentIndex - 1].NextTrigger = userClocks[currentIndex].NextTrigger;

            if (currentIndex < userClocks.Count - 1)
                userClocks[currentIndex + 1].LastTriggered = userClocks[currentIndex].LastTriggered;

            _clocks.Delete(currentIndex);
        }

        private void ChangeAlarm(ref int v, string obj, int offset, byte highBound)//clocks?
        {
            var newValue = v + offset;

            v = newValue == -1 ? highBound : (newValue == highBound + 1 ? 0 : newValue);

            OnPropertyChanged(obj);
            OnPropertyChanged(nameof(IsAllowedTime));

            _owner[0].OnPropertyChanged(nameof(IsAllowedTime));
        }

        private void AddAlarmClockExecute(object obj)
        {
            try
            {
                var alarm = new AlarmItem(_owner, _clocks, _hour, _minute);
                var clock = new Clock(StationManager.CurrentUser);

                _owner.Add(alarm);
                _clocks.Add(clock);

                var sortedList = _owner.Skip(1).OrderBy(GetTimeValue).ToList();

                var userClocks = _clocks.ForUser(StationManager.CurrentUser.Id);

                for (var i = 1; i < _owner.Count; i++)
                {
                    _owner[i] = sortedList[i - 1];

                    if (i > 1)
                        userClocks[i - 1].LastTriggered = GetNewClockTime(_owner[i - 1]._hour, _owner[i - 1]._minute);

                    if (i < _owner.Count - 1)
                        userClocks[i - 1].NextTrigger = GetNewClockTime(_owner[i + 1]._hour, _owner[i + 1]._minute);
                }
                //OnPropertyChanged(nameof(Clocks));
                OnPropertyChanged(nameof(IsAllowedTime));
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.CantParseTimeError);
            }
        }

        private int GetAlarmIndex()
        {
            for (var i = 1; i < _owner.Count; i++)
            {
                if (GetTimeValue(_owner[i]) == GetTimeValue(this))
                    return i;
            }

            return -1;
        }

        private static DateTime GetNewClockTime(int hour, int minute)
        {
            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        private void DoBellAlarm(object obj) => IsBlinking = !IsBlinking;

        private bool IsValidTime(string text, int param) =>
            !Regex.IsMatch(text) && text.Length == 2 &&
                int.Parse(text) >= 0 && int.Parse(text) <= param;
    }
}
