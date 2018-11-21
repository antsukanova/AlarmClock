using System;
using System.IO;
using System.Text;

namespace AlarmClock.Tools
{
    public static class Logger
    {
        public static void Log(string message)
        {
            lock (FileFolderHelper.LogFilepath)
            {
                StreamWriter writer = null;
                FileStream file = null;

                try
                {
                    FileFolderHelper.CheckAndCreateFile(FileFolderHelper.LogFilepath);

                    file   = new FileStream(FileFolderHelper.LogFilepath, FileMode.Append);
                    writer = new StreamWriter(file);

                    writer.WriteLine(DateTime.Now.ToString("HH:mm:ss.ms") + ": " + message);
                }
                catch
                {
                    // ignored
                }
                finally
                {
                    writer?.Close();
                    file?.Close();
                }
            }
        }
        public static void Log(Exception ex, string message = null)
        {
            var stringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(message))
                stringBuilder.AppendLine(message);

            while (ex != null)
            {
                stringBuilder.AppendLine(ex.Message);
                stringBuilder.AppendLine(ex.StackTrace);

                ex = ex.InnerException;
            }

            Log(stringBuilder.ToString());
        }
    }
}
