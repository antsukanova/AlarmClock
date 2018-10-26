using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
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
        private readonly AlarmItem currentAlarmItem;
        private string _currentTime;

        private Clock _selectedClock;

        private ICommand _signOut;
        #endregion

//        #region properties
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public string Hour = "11";// currentAlarmItem.Hour;
//        public string Minute { get => $"{_minute:00}"; }

        //#region commands

        //public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));

//        #endregion

        #region List of AlarmClocks
        public ObservableCollection<AlarmItem> Clocks { get; }
/*        public Clock SelectedClock
        {
            get => _selectedClock;
            set
            {
                _selectedClock = value;
                OnPropertyChanged();
            }
        }
*/
        #endregion

        #region command functions
        private void SignOutExecute(object obj)
        {
            StationManager.CurrentUser = null;

            NavigationManager.Navigate(Page.SignIn);
        }

        #endregion

        private DateTime GetNewClockTime()
        {
            // Ok if throws
          //  var hour = int.Parse(_hour);
          //  var minute = int.Parse(_minute);

            var tmpDate = DateTime.Now.AddDays(1);

            return new DateTime();//new DateTime(tmpDate.Year, tmpDate.Month, tmpDate.Day, hour, minute, tmpDate.Second);
        }


        public MainViewModel()
        {
            DateTime dt = DateTime.Now;
            Clocks = new ObservableCollection<AlarmItem>();

            SetTimer();
//            var clock = new Clock(dt, dt, StationManager.CurrentUser);
            Clocks.Add(new AlarmItem(Clocks, dt.Hour, dt.Minute));// clock);
                                                     //                OnPropertyChanged(nameof(Clocks));

            //            currentAlarmItem = new AlarmItem(null, dt.Hour, dt.Minute);
            //            _hour = dt.Hour;
            //            _minute = dt.Minute;
        }

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
