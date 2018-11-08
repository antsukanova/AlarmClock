using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AlarmClock.Misc;

namespace AlarmClock.Managers
{
    public static class SerializationManager
    {
        public static void Serialize<T>(T obj, string filePath)
        {
            try
            {
                FileFolderHelper.CheckAndCreateFile(filePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    new BinaryFormatter().Serialize(stream, obj);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, $"Failed to serialize data to file - {filePath}.");
                throw;
            }
        }

        public static T Deserialize<T>(string filePath) where T : class
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                    return (T) new BinaryFormatter().Deserialize(stream);
            }
            catch (Exception ex)
            {
                Logger.Log(ex, $"Failed to deserialize data from file - {filePath}.");
                return null;
            }
        }
    }
}
