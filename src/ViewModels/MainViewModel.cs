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
//        private static readonly Regex Regex = new Regex("[^0-9.-]+");
        private string _currentTime;

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

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));

        #region List of AlarmClocks
        public ObservableCollection<AlarmItem> Clocks { get; }
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
            Clocks = new ObservableCollection<AlarmItem>();

            SetTimer();
            Clocks.Add(new AlarmItem(Clocks, dt.Hour, dt.Minute));            
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
