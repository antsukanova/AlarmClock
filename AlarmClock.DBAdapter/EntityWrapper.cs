using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AlarmClock.DBModels;

namespace AlarmClock.DBAdapter
{
    public static class EntityWrapper
    {
        public static bool UserExists(string login) =>
            new ClockDBContext()
                .Users
                .Any(u => u.Login == login);

        public static User GetUserByLogin(string login) =>
            new ClockDBContext()
                .Users
                .Include(u => u.Clocks)
                .FirstOrDefault(u => u.Login == login);

        public static User GetUserByGuid(Guid guid) =>
            new ClockDBContext()
                .Users
                .Include(u => u.Clocks)
                .FirstOrDefault(u => u.Id == guid);

        public static List<User> GetAllUsers(Guid clockGuid) =>
                new ClockDBContext()
                    .Users
                    .Where(u => u.Clocks.All(r => r.Id != clockGuid))
                    .ToList();

        public static void AddUser(User user)
        {
            using (var context = new ClockDBContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void AddClock(Clock clock)
        {
            using (var context = new ClockDBContext())
            {
                clock.DeleteDatabaseValues();
                context.Clocks.Add(clock);
                context.SaveChanges();
            }
        }

        public static void SaveClock(Clock clock)
        {
            using (var context = new ClockDBContext())
            {
                context.Entry(clock).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        
        public static void DeleteClock(Clock selectedClock)
        {
            using (var context = new ClockDBContext())
            {
                selectedClock.DeleteDatabaseValues();
                context.Clocks.Attach(selectedClock);
                context.Clocks.Remove(selectedClock);
                context.SaveChanges();
            }
        }
    }
}
