using AlarmClock.DBModels;
using AlarmClock.Tools;

namespace AlarmClock.Managers
{
    public static class StationManager
    {
        public static User CurrentUser { get; private set; }

        static StationManager()
        {
            var user = SerializationManager.DeserializeLastUser();

            if (user == null)
                return;

            var realUser = DbManager.GetUserByLogin(user.Login);

            if (realUser != null && user.Password.Equals(realUser.Password))
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

        public static void Authorize(User user)
        {
            CurrentUser = user;
            Logger.Log($"User {CurrentUser.Login} was successfully authorized.");

            SerializationManager.SerializeCurrentUser();
        }

        public static void SignOut()
        {
            Logger.Log($"User {CurrentUser.Login} was successfully signed out.");
            CurrentUser = null;

            SerializationManager.ClearSerializedLastUser();
        }
    }
}
