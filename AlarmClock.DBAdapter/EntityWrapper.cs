using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using AlarmClock.DBModels;

namespace AlarmClock.DBAdapter
{
    public static class EntityWrapper
    {
        public static bool UserExists(string login)
        {
            using (var context = new ClockDbContext())
                return context.Users.Any(u => u.Login == login);
        }

        public static User GetUserByLogin(string login)
        {
            using (var context = new ClockDbContext())
                return context.Users.Include(u => u.Clocks).FirstOrDefault(u => u.Login == login);
        }

        public static User GetUserByGuid(Guid guid)
        {
            using (var context = new ClockDbContext())
                return context.Users.Include(u => u.Clocks).FirstOrDefault(u => u.Id == guid);
        }

        public static List<User> GetAllUsers(Guid clockGuid)
        {
            using (var context = new ClockDbContext())
                return context.Users.Where(u => u.Clocks.All(r => r.Id != clockGuid)).ToList();
        }

        public static void AddUser(User user)
        {
            using (var context = new ClockDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public static void UpdateUser(User user)
        {
            using (var context = new ClockDbContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public static Clock AddClock(Clock clock)
        {
            using (var context = new ClockDbContext())
            {
                clock.DeleteDatabaseValues();

                context.Clocks.Add(clock);

                context.SaveChanges();
            }

            return clock;
        }

        public static void SaveClock(Clock clock)
        {
            using (var context = new ClockDbContext())
            {
                context.Entry(clock).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        
        public static void DeleteClock(Clock selectedClock)
        {
            using (var context = new ClockDbContext())
            {
                selectedClock.DeleteDatabaseValues();

                context.Clocks.Attach(selectedClock);
                context.Clocks.Remove(selectedClock);

                context.SaveChanges();
            }
        }
    }
}
