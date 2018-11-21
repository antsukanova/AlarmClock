using System;
using System.ServiceModel;
using System.ServiceProcess;
using AlarmClock.Tools;

namespace AlarmClock.ClockService
{
    public class ClockSimulatorWindowsService : ServiceBase
    {
        internal const string CurrentServiceName = "ClockSimulatorService";
        internal const string CurrentServiceDisplayName = "Clock Simulator Service";
        internal const string CurrentServiceDescription = "Clock Simulator for learning purposes.";
        private ServiceHost _serviceHost;

        public ClockSimulatorWindowsService()
        {
            ServiceName = CurrentServiceName;

            try
            {
                AppDomain.CurrentDomain.UnhandledException += UnhandledException;
                Logger.Log("Initialization");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Initialization");
            }
        }

        protected override void OnStart(string[] args)
        {
            Logger.Log("OnStart");
            RequestAdditionalTime(60 * 1000);

        #if DEBUG
            //for (int i = 0; i < 100; i++)
            //{
            //    Thread.Sleep(1000);
            //}
        #endif

            try
            {
                _serviceHost?.Close();
            }
            catch
            {
                // ignore
            }

            try
            {
                _serviceHost = new ServiceHost(typeof(ClockSimulatorService));
                _serviceHost.Open();
            }
            catch (Exception ex)
            {
                Logger.Log(ex, nameof(OnStart));
                throw;
            }

            Logger.Log("Service Successfully Started");
        }

        protected override void OnStop()
        {
            Logger.Log(nameof(OnStop));
            RequestAdditionalTime(60 * 1000);

            try
            {
                _serviceHost.Close();
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Trying To Stop The Host Listener");
            }

            Logger.Log("Service Stopped");
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs args) => 
            Logger.Log((Exception)args.ExceptionObject, nameof(UnhandledException));
    }
}
