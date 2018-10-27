using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
using AlarmClock.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AlarmClock.Models
{
    public class AlarmItem : NotifyPropertyChanged
    {
        private static readonly Regex regex = new Regex("[^0-9.-]+");
        private readonly byte maxMinutes = (byte)(TimeSpan.TicksPerMinute / TimeSpan.TicksPerSecond) - 1;
        private readonly byte maxHours = (byte)(TimeSpan.TicksPerDay / TimeSpan.TicksPerHour) - 1;
        private ObservableCollection<AlarmItem> _owner;
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

        public AlarmItem(ObservableCollection<AlarmItem> owner, ClockRepository clocks, int hour, int minute)
        {
            _owner = owner;
            _clocks = clocks;
            _hour = hour;
            _minute = minute;
        }

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), 1, maxHours); }
            ));

        public ICommand ClickDownHour =>
            _clickDownHour ??
           (_clickDownHour = new RelayCommand(
                delegate { ChangeAlarm(ref _hour, nameof(Hour), -1, maxHours); }
           ));

        public ICommand ClickUpMinute =>
            _clickUpMinute ??
           (_clickUpMinute = new RelayCommand(
               delegate { ChangeAlarm(ref _minute, nameof(Minute), 1, maxMinutes); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ??
           (_clickDownMinute = new RelayCommand(
                delegate { ChangeAlarm(ref _minute, nameof(Minute), -1, maxMinutes); }
           ));

        public ICommand AddAlarm => _addAlarm ?? (_addAlarm = new RelayCommand(AddAlarmClockExecute));

        public ICommand DeleteAlarm => _deleteAlarm ?? (_deleteAlarm = new RelayCommand(DoDeleteAlarm));

        public ICommand BellAlarm => _bellAlarm ?? (_bellAlarm = new RelayCommand(DoBellAlarm));
        #endregion

        private int GetTimeValue(AlarmItem ai) => ai._hour * maxMinutes + ai._minute;
        private void DoDeleteAlarm(object obj)
        {
            int currentIndex = GetAlarmIndex() - 1;

            _owner.Remove(this);

            if (currentIndex > 0)
                _clocks.ForUser()[currentIndex - 1].NextTrigger = _clocks.ForUser()[currentIndex].NextTrigger;
            if (currentIndex < _clocks.ForUser().Count())
                _clocks.ForUser()[currentIndex + 1].LastTriggered = _clocks.ForUser()[currentIndex].LastTriggered;
            _clocks.Delete(currentIndex);
        }

        private void DoBellAlarm(object obj) => MessageBox.Show("Ringing...");

        private void ChangeAlarm(ref int v, string obj, int offset, byte highBound)//clocks?
        {
            var newValue = v + offset;

            v = newValue == -1 ? highBound : newValue == highBound + 1 ? 0 : newValue;
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
                var sordedList = _owner
                    .Skip(1)
                    .OrderBy(item => GetTimeValue(item))
                    .ToList();
                for (int i = 1; i < _owner.Count(); i++)
                {
                    _owner[i] = sordedList[i - 1];
                    // _clocks.ForUser(StationManager.CurrentUser.Id);
                    if (i > 1)
                        _clocks.ForUser()[i - 1].LastTriggered = GetNewClockTime(_owner[i - 1]._hour, _owner[i - 1]._minute);
                    if (i < _owner.Count() - 1)
                        _clocks.ForUser()[i - 1].NextTrigger = GetNewClockTime(_owner[i + 1]._hour, _owner[i + 1]._minute);
                }
                //OnPropertyChanged(nameof(Clocks));
                OnPropertyChanged(nameof(IsAllowedTime));
                //for testing _clocks
/*                for (int g = 0; g < _clocks.ForUser().Count(); g++)
                {
                    Clock c = _clocks.ForUser()[g];
                    int f;
                    f = 22;
                }
*/            }
            catch (Exception)
            {
                MessageBox.Show(Resources.CantParseTimeError);
                return;
            }

            // validate time
            // if not correct -> say it
            // add clock to 'db' via ClockRepository
            // reload clocks list OR just add created clock to it
        }

        private int GetAlarmIndex()
        {
            for (int i = 1; i < _owner.Count(); i++)
                if (GetTimeValue(_owner[i]) == GetTimeValue(this))
                    return i;

            return -1;
        }

        private DateTime GetNewClockTime(int hour, int minute)
        {
            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        public string Hour
        {
            get => $"{_hour:00}";
            set
            {
                if (IsValidTime(value, maxHours))
                {
                    _hour = int.Parse(value);
                    OnPropertyChanged(nameof(Hour));
                    OnPropertyChanged(nameof(IsAllowedTime));
                }
            }
        }

        public string Minute
        {
            get => $"{_minute:00}";
            set
            {
                if (IsValidTime(value, maxMinutes))
                {
                    _minute = int.Parse(value);
                    OnPropertyChanged(nameof(Minute));
                    OnPropertyChanged(nameof(IsAllowedTime));
                }
            }
        }

        public bool IsBaseAlarm { get => _owner.IndexOf(this) == 0; }
        public bool IsAddEnabled { get => IsBaseAlarm; }
        public bool IsSaveEnabled { get => !IsBaseAlarm; }
        public bool IsCancelEnabled { get => !IsBaseAlarm; }
        public bool IsBellEnabled { get => !IsBaseAlarm; }
        public bool IsDeleteEnabled { get => !IsBaseAlarm; }
        public bool IsAllowedTime
        {
            get => !_owner
                .Skip(1)
                .Where(item => item != this)
                .Select(item => GetTimeValue(item))
                .Contains(GetTimeValue(this));
        }

        private bool IsValidTime(string text, int param) =>
            !regex.IsMatch(text) && text.Length == 2 &&
                int.Parse(text) >= 0 && int.Parse(text) <= param;
    }
}
