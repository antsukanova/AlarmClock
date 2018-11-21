using AlarmClock.DBModels;
using AlarmClock.ServiceInterface;
using System.Collections.Generic;

namespace AlarmClock.Managers
{
    public static class DbManager
    {
        public static bool UserExists(string login) => 
            ClockServiceWrapper.UserExists(login);

        public static User GetUserByLogin(string login) => 
            ClockServiceWrapper.GetUserByLogin(login);

        public static void AddUser(User user) => ClockServiceWrapper.AddUser(user);

        public static void UpdateUser(User user) => ClockServiceWrapper.UpdateUser(user);

        public static void DeleteClock(Clock selectedClock) => 
            ClockServiceWrapper.DeleteClock(selectedClock);

        public static Clock AddClock(Clock clock) => ClockServiceWrapper.AddClock(clock);

        public static List<Clock> GetClocksByUser(User user) =>
            ClockServiceWrapper.GetUserByGuid(user.Id).Clocks;

        public static void SaveClock(Clock clock) => ClockServiceWrapper.SaveClock(clock);
    }
}

