using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Models;
using AlarmClock.Properties;

namespace AlarmClock.ViewModels
{
    class MainViewModel : NotifyPropertyChanged
    {
        #region attributes
        private static readonly Regex Regex = new Regex("[^0-9.-]+");

        private string _currentTime;

        private string _textHour = "00";
        private string _textMinute = "00";

        private ICommand _clickUpHour;
        private ICommand _clickDownHour;
        private ICommand _clickUpMinute;
        private ICommand _clickDownMinute;

        private ICommand _signOut;
        private ICommand _addAlarmClock;
        #endregion

        #region properties
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public string TextHour
        {
            get => _textHour;
            set
            {
                if (IsValidTime(value, 23))
                {
                    _textHour = value;
                    OnPropertyChanged(nameof(TextHour));
                }
            }
        }

        public string TextMinute
        {
            get => _textMinute;
            set
            {
                if (IsValidTime(value, 59))
                {
                    _textMinute = value;
                    OnPropertyChanged(nameof(TextMinute));
                }
            }
        }

        #region commands
        public ICommand ClickUpHour =>
            _clickUpHour ??
           (_clickUpHour = new RelayCommand(
                delegate { SpinChange(ref _textHour, nameof(TextHour), 1, 23); }
            ));

        public ICommand ClickDownHour => 
            _clickDownHour ?? 
           (_clickDownHour = new RelayCommand(
                delegate { SpinChange(ref _textHour, nameof(TextHour), -1, 23); }
           ));

        public ICommand ClickUpMinute => 
            _clickUpMinute ?? 
           (_clickUpMinute = new RelayCommand(
               delegate { SpinChange(ref _textMinute, nameof(TextMinute), 1, 59); }
           ));

        public ICommand ClickDownMinute =>
            _clickDownMinute ?? 
           (_clickDownMinute = new RelayCommand(
                delegate { SpinChange(ref _textMinute, nameof(TextMinute), -1, 59); }
           ));

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));

        public ICommand AddAlarmClock => 
            _addAlarmClock ?? 
           (_addAlarmClock = new RelayCommand(AddAlarmClockExecute));
        #endregion
        #endregion

        #region command functions
        private void SpinChange(ref string v, string obj, int offset, int highBound)
        {
            var newValue = int.Parse(v) + offset;

            v = $"{(newValue == -1 ? highBound : newValue == highBound + 1 ? 0 : newValue):00}";

            OnPropertyChanged(obj);
        }

        private static void SignOutExecute(object obj)
        {
            StationManager.CurrentUser = null;

            NavigationManager.Navigate(Page.SignIn);
        }

        private static void AddAlarmClockExecute(object obj)
        {
            try
            {
                // create new clock for user (use GetNewClockTime)
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
        #endregion

        private DateTime GetNewClockTime()
        {
            // Ok if throws
            var hour = int.Parse(_textHour);
            var minute = int.Parse(_textMinute);

            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }

        private static bool IsValidTime(string text, int param) 
            => !Regex.IsMatch(text) && text.Length == 2 &&
               int.Parse(text) >= 0 && int.Parse(text) <= param;

        public MainViewModel() => SetTimer();

        private void SetTimer()
        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e) =>
            CurrentTime = DateTime.Now.ToString("H:mm:ss");
    }
}
