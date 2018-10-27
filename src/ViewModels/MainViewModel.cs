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
using AlarmClock.Repositories;

namespace AlarmClock.ViewModels
{
    class MainViewModel : NotifyPropertyChanged
    {
        #region attributes
        private string _currentTime;

        private ICommand _signOut;
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

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));
        #endregion

        #region List of AlarmClocks
        public ObservableCollection<AlarmItem> AlarmClocks { get; }
        public ClockRepository Clocks { get; }
        #endregion

        #region command functions
        private void SignOutExecute(object obj)
        {
            StationManager.CurrentUser = null;

            NavigationManager.Navigate(Page.SignIn);
        }

        #endregion

        public MainViewModel()
        {
            DateTime dt = DateTime.Now;
            AlarmClocks = new ObservableCollection<AlarmItem>();
            Clocks = new ClockRepository();

            SetTimer();
            AlarmClocks.Add(new AlarmItem(AlarmClocks, Clocks, dt.Hour, dt.Minute));            
        }
        
        private void SetTimer()
        {
            var timer = new DispatcherTimer();

            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            CurrentTime = dt.ToString("H:mm:ss");
//            AlarmClocks.Con
        }
    }
}