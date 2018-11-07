using System.Windows;

using AlarmClock.Misc;

namespace AlarmClock
{
    public partial class AlarmClockApp
    {
        private AlarmClockApp() => Logger.Log("App started.");

        private void OnExit(object sender, ExitEventArgs e) => Logger.Log("App closed.");
    }
}
