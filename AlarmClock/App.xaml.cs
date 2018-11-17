using System.Windows;

using AlarmClock.Managers;
using AlarmClock.Repositories;
using AlarmClock.Tools;
using AlarmClock.ViewModels;

namespace AlarmClock
{
    public partial class AlarmClockApp
    {
        private AlarmClockApp() => Logger.Log("App started.");

        private void OnExit(object sender, ExitEventArgs e)
        {
            SerializationManager.SerializeAlarms(new ClockRepository().All());
            if (MainViewModel.CurrentUser != null)
                MainViewModel.SaveClock();

            Logger.Log("App closed.");
        }
    }
}
