using System;
using System.IO;

namespace AlarmClock.Tools
{
    public static class FileFolderHelper
    {
        private static readonly string AppDataPath =
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private static readonly string ClientFolderPath =
            Path.Combine(AppDataPath, "AlarmClock");

        private static readonly string LogFolderPath =
            Path.Combine(ClientFolderPath, "Log");

        internal static readonly string LogFilepath = Path.Combine(
            LogFolderPath, $"Logs_{DateTime.Now:yyyy_MM_dd}.txt"
        );

        public static readonly string AlarmsFilePath =
            Path.Combine(ClientFolderPath, "Alarms.alcl");

        public static readonly string UsersFilePath =
            Path.Combine(ClientFolderPath, "Users.alcl");

        public static readonly string LastUserFilePath =
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