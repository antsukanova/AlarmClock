using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Models;
using AlarmClock.Repositories;

namespace AlarmClock.ViewModels
{
    internal class MainViewModel : NotifyPropertyChanged
    {
        #region attributes
        private string _currentTime;
        private DispatcherTimer _setTimer;
        private DispatcherTimer _checkAlarm;
        private static readonly object Lock = new object();

        private ICommand _signOut;
        private ICommand _alarmCompleted;
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

        public ICommand AlarmCompleted => _alarmCompleted ?? (_alarmCompleted = new RelayCommand(
            delegate
            {
                var alarm = AlarmClocks.SingleOrDefault(item => item.IsActive);
                if (alarm != null)
                    alarm.IsStopped = true;
            }));
        #endregion

        #region command functions
        private void SignOutExecute(object obj)
        {
            _setTimer.Stop();
            _checkAlarm.Stop();

            StationManager.SignOut();

            NavigationManager.Navigate(Page.SignIn);
        }
        #endregion

        public MainViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(AlarmClocks, Lock);
            StartUp();
        }

        private async void StartUp()
        {
            LoaderManager.Instance.ShowLoader();
            (_setTimer = SetTimer()).Start();
            (_checkAlarm = CheckAlarm()).Start();
            await Task.Run(() =>
            {
                var now = DateTime.Now;
                
                AlarmClocks.Add(new AlarmItem(AlarmClocks, Clocks, now.Hour, now.Minute));

                Clocks
                    .ForUser(CurrentUser.Id)
                    .ForEach(clock => AlarmClocks[0].AddAlarm.Execute(clock));

                Logger.Log($"Loaded Alarm clocks for User {CurrentUser.Login}.");

                return true;
            });
            LoaderManager.Instance.HideLoader();
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
                Interval = TimeSpan.FromSeconds(1)
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
                    ai.Clock.NextTrigger = ai.Clock.LastTriggered.AddDays(1);
                    break;
                }
            };

            return timer;
        }
    }
}