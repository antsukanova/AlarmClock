//using AlarmClock.Annotations;
//using AlarmClock.Misc;

using System;
using System.Reflection;
using AlarmClock.DBModels;
//using AlarmClock.Repositories;
using AlarmClock.Tools;

namespace AlarmClock.Managers
{
    public static class StationManager<T>
    {
        public static User CurrentUser { get; private set; }

        static StationManager()
        {
            var user = SerializationManager.DeserializeLastUser();

            if (user == null)
                return;

            var type = typeof(T);
            var method = type.GetMethod("Find");
            if (method == null)
                return;
            var realUser = method.Invoke(Activator.CreateInstance(type, null), new object[] { user.Login });
            //            var realUser = new UserRepository().Find(user.Login);

//            if (realUser != null && user.Password.Equals(realUser.Password))
            if (realUser != null && user.Password.Equals(realUser.GetType().GetProperty("Password")?.GetValue(realUser, null)))
            {
                CurrentUser = user;
                Logger.Log($"Deserialized User {CurrentUser.Login} was successfully authorized.");
            }
            else
            {
                Logger.Log("Deserialized user was not found in the db.");
                SerializationManager.ClearSerializedLastUser();
            }
        }

        public static void Authorize(User user)//[NotNull] User user
        {
            CurrentUser = user;
            Logger.Log($"User {CurrentUser.Login} was successfully authorized.");

            SerializationManager.SerializeCurrentUser<T>();
        }

        public static void SignOut()
        {
            Logger.Log($"User {CurrentUser.Login} was successfully signed out.");
            CurrentUser = null;

            SerializationManager.ClearSerializedLastUser();
        }
    }
}
