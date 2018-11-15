using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AlarmClock.Misc
{
    public static class SerializationHelper
    {
        public static void Serialize<T>(T obj, string filePath)
        {
            FileFolderHelper.CheckAndCreateFile(filePath);

            using (var stream = new FileStream(filePath, FileMode.Create))
                new BinaryFormatter().Serialize(stream, obj);
        }

        public static T Deserialize<T>(string filePath) where T : class
        {
            using (var stream = new FileStream(filePath, FileMode.Open))
                return (T) new BinaryFormatter().Deserialize(stream);
        }
    }
}
