using System;
using System.Configuration.Install;
using System.Reflection;
using System.Windows.Forms;

namespace AlarmClock.ClockService
{
    internal static class SelfInstaller
    {
        private static readonly string ExePath = Assembly.GetExecutingAssembly().Location;

        internal static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] {ExePath});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }

        internal static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] {"/u", ExePath});
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            return true;
        }
    }
}