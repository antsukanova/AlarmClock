using System.ServiceProcess;
using System.Windows.Forms;

namespace AlarmClock.ClockService
{
    internal class Program
    {
        private static void Main()
        {
            var isInstalled = false;
            var serviceStarting = false;

            const string serviceName = ClockSimulatorWindowsService.CurrentServiceName;

            var services = ServiceController.GetServices();

            foreach (var service in services)
            {
                if (!service.ServiceName.Equals(serviceName))
                    continue;

                isInstalled = true;

                if (service.Status == ServiceControllerStatus.StartPending)
                    serviceStarting = true;

                break;
            }

            if (!serviceStarting)
            {
                if (isInstalled)
                {
                    var dialogResult = MessageBox.Show(
                        $"Do You REALLY Want To Uninstall {serviceName}", "Danger",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning
                    );

                    if (dialogResult != DialogResult.Yes)
                        return;

                    SelfInstaller.UninstallMe();

                    MessageBox.Show(
                        $"{serviceName} Successfully Uninstalled", "Status", MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                }
                else
                {
                    MessageBox.Show(
                        SelfInstaller.InstallMe() ? $"{serviceName} Successfully Installed" :
                                                    $"{serviceName} Failed To Install", 
                        "Status", MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] { new ClockSimulatorWindowsService() });
            }
        }
    }
}
