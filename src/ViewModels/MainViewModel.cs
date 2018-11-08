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

        public static User CurrentUser => StationManager.CurrentUser;

        public ICommand SignOut => _signOut ?? (_signOut = new RelayCommand(SignOutExecute));

        private AlarmItem BaseAlarm => AlarmClocks.Single(item => item.IsBaseAlarm);

        public void Changed()
        {
            foreach (var ai in AlarmClocks.Where(item => item != BaseAlarm))
                ai.IsVisible = ai.Clock.Owner == (StationManager.CurrentUser ?? ai.Clock.Owner);

            BaseAlarm.Update();
        }
        #endregion

        #region command functions
        private void SignOutExecute(object obj)
        {
            BaseAlarm.Rearrange();

            Logger.Log($"User {CurrentUser.Login} was successfully signed out.");

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

            // TODO: Actually load clocks for the User
            Logger.Log($"Loaded Alarm clocks for User {CurrentUser.Login}.");
        }

        private DispatcherTimer SetTimer()
        {
            var timer = new DispatcherTimer
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
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(200)
            };

            timer.Tick += delegate
            {
                var dt = DateTime.Now;
                var alarms = AlarmClocks[0].UserAlarms;

                if (alarms.Any(item => item.IsActive))
                    return;

                foreach (var ai in alarms)
                {
                    if (!ai.Equals(dt) || ai.IsStopped)
                        continue;

                    ai.IsActive = true;
                    ai.Clock.LastTriggered = DateTime.Now;
                    break;
                }
            };

            return timer;
        }
    }
}