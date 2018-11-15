using System;
using System.Collections.Generic;
using System.IO;

using AlarmClock.Misc;
using AlarmClock.Models;
using AlarmClock.Repositories;

namespace AlarmClock.Managers
{
    public static class SerializationManager
    {
    #region Last User
        public static void SerializeCurrentUser()
        {
            try
            {
                SerializationHelper.Serialize(StationManager.CurrentUser, FileFolderHelper.LastUserFilePath);
                Logger.Log("Current user was successfully serialized.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to serialize current user.");
            }
        }

        public static User DeserializeLastUser()
        {
            User user = null;

            try
            {
                user = SerializationHelper.Deserialize<User>(FileFolderHelper.LastUserFilePath);
                Logger.Log("Last user was successfully deserialized.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to deserialize last User.");
            }

            return user;
        }

        internal static void ClearSerializedLastUser()
        {
            try
            {
                using (var stream = new FileStream(FileFolderHelper.LastUserFilePath, FileMode.Open))
                    stream.SetLength(0);

                Logger.Log("Serialized last user was successfully cleared.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to clear serialized last User.");
            }
        }
        #endregion

        #region Alarms
        public static void SerializeAlarms(IClockRepository repo)
        {
            try
            {
                SerializationHelper.Serialize(repo.All(), FileFolderHelper.AlarmsFilePath);
                Logger.Log("Alarm clocks were successfully serialized.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to serialize alarm clocks.");
            }
        }

        public static List<Clock> DeserializeAlarms()
        {
            try
            {
                var clocks = SerializationHelper.Deserialize<List<Clock>>(FileFolderHelper.AlarmsFilePath);
                Logger.Log("Alarm clocks were successfully deserialized.");

                return clocks;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to deserialize alarm clocks.");
                return null;
            }
        }

        internal static void ClearSerializedAlarms()
        {
            try
            {
                using (var stream = new FileStream(FileFolderHelper.AlarmsFilePath, FileMode.Open))
                    stream.SetLength(0);

                Logger.Log("Serialized alarms were successfully cleared.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to clear serialized alarms.");
            }
        }
        #endregion

        #region Users
        public static void SerializeUsers(IUserRepository repo)
        {
            try
            {
                SerializationHelper.Serialize(repo.All(), FileFolderHelper.UsersFilePath);
                Logger.Log("Users were successfully serialized.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to serialize users.");
            }
        }

        public static List<User> DeserializeUsers()
        {
            try
            {
                var users = SerializationHelper.Deserialize<List<User>>(FileFolderHelper.UsersFilePath);
                Logger.Log("Users were successfully deserialized.");

                return users;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to deserialize users.");
                return null;
            }
        }

        internal static void ClearSerializedUsers()
        {
            try
            {
                using (var stream = new FileStream(FileFolderHelper.UsersFilePath, FileMode.Open))
                    stream.SetLength(0);

                Logger.Log("Serialized users were successfully cleared.");
            }
            catch (Exception ex)
            {
                Logger.Log(ex, "Failed to clear serialized users.");
            }
        }
        #endregion
    }
}
