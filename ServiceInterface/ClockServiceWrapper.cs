using System;
using System.ServiceModel;
using AlarmClock.DBModels;

namespace AlarmClock.ServiceInterface
{
    public class ClockServiceWrapper
    {
        public static bool UserExists(string login)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                return channelFactory.CreateChannel().UserExists(login);
        }

        public static User GetUserByLogin(string login)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                return channelFactory.CreateChannel().GetUserByLogin(login);
        }

        public static User GetUserByGuid(Guid guid)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                return channelFactory.CreateChannel().GetUserByGuid(guid);
        }

        public static void AddUser(User user)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                channelFactory.CreateChannel().AddUser(user);
        }

        public static void UpdateUser(User user)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                channelFactory.CreateChannel().UpdateUser(user);
        }

        public static Clock AddClock(Clock clock)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                return channelFactory.CreateChannel().AddClock(clock);
        }

        public static void SaveClock(Clock clock)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                channelFactory.CreateChannel().SaveClock(clock);
        }

        public static void DeleteClock(Clock selectedClock)
        {
            using (var channelFactory = new ChannelFactory<IClockContract>("Server"))
                channelFactory.CreateChannel().DeleteClock(selectedClock);
        }
    }
}

