using System.Windows;

using AlarmClock.Tools;
using AlarmClock.ViewModels;

namespace AlarmClock
{
    public partial class AlarmClockApp
    {
        private AlarmClockApp() => Logger.Log("App started.");

        private void OnExit(object sender, ExitEventArgs e)
        {
            if (MainViewModel.CurrentUser != null)
                MainViewModel.SaveClocks();

            Logger.Log("App closed.");
        }
    }
}
