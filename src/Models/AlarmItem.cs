using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Properties;
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
        private readonly ObservableCollection<AlarmItem> _owner;

        private int _hour;
        private int _minute;
        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;
        private ICommand _addAlarm;
        private ICommand _deleteAlarm;
        private ICommand _bellAlarm;

        public AlarmItem(ObservableCollection<AlarmItem> owner, int hour, int minute)
        {
            _owner = owner;
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

        private void DoDeleteAlarm(object obj) => _owner.Remove(this);

        private void DoBellAlarm(object obj) => MessageBox.Show("Ringing...");

        private void ChangeAlarm(ref int v, string obj, int offset, byte highBound)
        {
            var newValue = v + offset;

            v = newValue == -1 ? highBound : newValue == highBound + 1 ? 0 : newValue;
            OnPropertyChanged(obj);
            OnPropertyChanged(nameof(IsAllowedTime));
        }

        private void AddAlarmClockExecute(object obj)
        {
            try
            {
                // create new clock for user (use GetNewClockTime)
//                DateTime dt = GetNewClockTime();

//                var clock = new Clock(dt, dt, StationManager.CurrentUser);
                _owner.Add(new AlarmItem(_owner, _hour, _minute));// clock);
                                                                  //                OnPropertyChanged(nameof(Clocks));

                //                _selectedClock = clock;
                //                CollectionViewSource.GetDefaultView(Clocks).Refresh();

                //    _textHour = dt.Hour.ToString();
                //    _textMinute = dt.Minute.ToString();
                OnPropertyChanged(nameof(IsAllowedTime));
            }
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

        private DateTime GetNewClockTime()
        {
            // Ok if throws
            //  var hour = int.Parse(_hour);
            //  var minute = int.Parse(_minute);

            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime();// tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
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
                .Select(item => new { item._hour, item._minute })
                .Contains(new { _hour, _minute });
        }

        private bool IsValidTime(string text, int param) =>
            !regex.IsMatch(text) && text.Length == 2 &&
                int.Parse(text) >= 0 && int.Parse(text) <= param;
    }
}
