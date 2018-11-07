using System;
using System.IO;

namespace AlarmClock.Misc
{
    internal static class FileFolderHelper
    {
        private static readonly string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        internal static readonly string ClientFolderPath =
            Path.Combine(AppDataPath, "AlarmClock");

        internal static readonly string LogFolderPath =
            Path.Combine(ClientFolderPath, "Log");

        internal static readonly string LogFilepath = Path.Combine(
            LogFolderPath, $"Logs_{DateTime.Now:yyyy_MM_dd}.txt"
        );

        internal static readonly string UserAlarmsFilePath =
            Path.Combine(ClientFolderPath, "UserAlarms.alcl");

        internal static readonly string LastUserFilePath =
            Path.Combine(ClientFolderPath, "LastUser.alcl");

        internal static void CheckAndCreateFile(string filePath)
        {
            var file = new FileInfo(filePath);

            if (file.Directory != null && !file.Directory.Exists)
                file.Directory.Create();

            if (!file.Exists)
                file.Create().Close();
        }
    }
}