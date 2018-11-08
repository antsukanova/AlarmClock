using System.Windows;

using AlarmClock.Managers;
using AlarmClock.Misc;
using AlarmClock.Repositories;

namespace AlarmClock
{
    public partial class AlarmClockApp
    {
        private AlarmClockApp() => Logger.Log("App started.");

        private void OnExit(object sender, ExitEventArgs e)
        {
            SerializationManager.SerializeAlarms(new ClockRepository());
            Logger.Log("App closed.");
        }
    }
}
