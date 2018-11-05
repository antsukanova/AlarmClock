using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        public User CurrentUser => StationManager.CurrentUser;

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));

        private AlarmItem BaseAlarm => AlarmClocks.Where(item => item.IsBaseAlarm).Single();

        public void Changed()
        {
            OnPropertyChanged(nameof(CurrentUser));

            foreach (AlarmItem ai in AlarmClocks.Where(item => item != BaseAlarm))
                ai.IsVisible = ai.Clock.Owner == (StationManager.CurrentUser ?? ai.Clock.Owner);

            BaseAlarm.Update();
        }
        #endregion

        #region command functions
        private void SignOutExecute(object obj)
        {
            BaseAlarm.Rearrange();

            StationManager.CurrentUser = null;

            NavigationManager.Navigate(Page.SignIn);
        }
        #endregion

        public MainViewModel()
        {
            var now = DateTime.Now;

            AlarmClocks.Add(new AlarmItem(AlarmClocks, Clocks, now.Hour, now.Minute));

            Changed();

            SetTimer().Start();
            CheckAlarm().Start();
        }

        private DispatcherTimer SetTimer()
        {
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += delegate
            {
                CurrentTime = DateTime.Now.ToString("H:mm:ss");
            };

            return timer;
        }

        private DispatcherTimer CheckAlarm()
        {
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            timer.Tick += delegate
            {
                DateTime dt = DateTime.Now;
                var userAlarms = AlarmClocks[0].UserAlarms;

                foreach (AlarmItem ai in userAlarms)
                {
                    if (ai.Equals(dt) && !ai.IsStopped && !userAlarms.Any(item => item.IsActive))
                        ai.IsActive = true;
                }
            };

            return timer;
        }
    }
}