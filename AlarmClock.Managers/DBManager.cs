using AlarmClock.DBModels;
using AlarmClock.DBAdapter;
using System.Collections.Generic;

namespace AlarmClock.Managers
{
    public class DBManager
    {
        public static bool UserExists(string login) => EntityWrapper.UserExists(login);

        public static User GetUserByLogin(string login) => EntityWrapper.GetUserByLogin(login);

        public static void AddUser(User user)
        {
            EntityWrapper.AddUser(user);
        }

        internal static User CheckCachedUser(User userCandidate)
        {
            var userInStorage = EntityWrapper.GetUserByGuid(userCandidate.Id);
            if (userInStorage != null && userInStorage.IsPasswordCorrect(userCandidate))
                return userInStorage;
            return null;
        }
        
        public static void DeleteClock(Clock selectedClock)
        {
            EntityWrapper.DeleteClock(selectedClock);
        }

        public static void AddClock(Clock clock)
        {
            EntityWrapper.AddClock(clock);
        }

        public static List<Clock> GetClocksByUser(User user) =>
            EntityWrapper.GetUserByGuid(user.Id).Clocks;

        public static void SaveClock(Clock selectedClock)
        {
            EntityWrapper.SaveClock(selectedClock);
        }
    }
}

