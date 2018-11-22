using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace AlarmClock.ClockService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            var serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem, Password = null, Username = null
            };

            var serviceInstaller = new ServiceInstaller
            {
                ServiceName = ClockSimulatorWindowsService.CurrentServiceName,
                DisplayName = ClockSimulatorWindowsService.CurrentServiceDisplayName,
                Description = ClockSimulatorWindowsService.CurrentServiceDescription,
                StartType   = ServiceStartMode.Automatic
            };

            Installers.AddRange(new Installer[] {serviceProcessInstaller, serviceInstaller});
        }
    }
}
