using AlarmClock.Annotations;
using AlarmClock.Models;

namespace AlarmClock.Managers
{
    public static class StationManager
    {
        public static User CurrentUser { get; private set; }

        static StationManager()
        {
            var user = SerializationManager.DeserializeLastUser();

            // TODO: check if user is in the db
            if (user != null)
            {
                CurrentUser = user;
            }
        }

        public static void Authorize([NotNull] User user)
        {
            CurrentUser = user;
            SerializationManager.SerializeCurrentUser();
        }

        public static void SignOut()
        {
            CurrentUser = null;
            SerializationManager.ClearSerializedLastUser();
        }
    }
}
