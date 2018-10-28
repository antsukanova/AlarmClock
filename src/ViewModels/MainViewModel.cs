using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Models;
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

        public ObservableCollection<AlarmItem> AlarmClocks { get; } =
            new ObservableCollection<AlarmItem>();

        public ClockRepository Clocks { get; } = new ClockRepository();

        public static User CurrentUser { get; } = StationManager.CurrentUser;

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));
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
            SetTimer();

            var now = DateTime.Now;
            AlarmClocks.Add(new AlarmItem(AlarmClocks, Clocks, now.Hour, now.Minute));            
        }
        
        private void SetTimer()
        {
            var timer = new DispatcherTimer();

            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e) =>
            CurrentTime = DateTime.Now.ToString("H:mm:ss");
    }
}