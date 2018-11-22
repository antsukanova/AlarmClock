using System;
using AlarmClock.DBAdapter;
using AlarmClock.DBModels;
using AlarmClock.ServiceInterface;

namespace AlarmClock.ClockService
{
    class ClockSimulatorService: IClockContract
    {
        public bool UserExists(string login) => EntityWrapper.UserExists(login);

        public User GetUserByLogin(string login) => EntityWrapper.GetUserByLogin(login);

        public User GetUserByGuid(Guid guid) => EntityWrapper.GetUserByGuid(guid);

        public void AddUser(User user) => EntityWrapper.AddUser(user);

        public void UpdateUser(User user) => EntityWrapper.UpdateUser(user);

        public Clock AddClock(Clock clock) => EntityWrapper.AddClock(clock);

        public void SaveClock(Clock clock) => EntityWrapper.SaveClock(clock);

        public void DeleteClock(Clock clock) => EntityWrapper.DeleteClock(clock);
    }
}
