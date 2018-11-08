using AlarmClock.Models;

namespace AlarmClock.Managers
{
    public static class StationManager
    {
        private static User _currentUser;

        public static User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                SerializationManager.SerializeCurrentUser();
            }
        }

        static StationManager()
        {
            var user = SerializationManager.DeserializeLastUser();

            // TODO: check if user is in the db
            if (user != null)
            {
                CurrentUser = user;
            }
        }

    }
}
