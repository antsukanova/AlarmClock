using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using AlarmClock.DBAdapter.Properties;
using AlarmClock.DBModels;

namespace AlarmClock.DBAdapter
{
    public static class EntityWrapper
    {
        #region Users
        public static bool UserExists(string login)
        {
            try
            {
                using (var context = new ClockDbContext())
                    return context.Users.Any(u => u.Login == login);
            }
            catch (Exception)
            {
                throw new InvalidOperationException(Resources.InvalidOperationException);
            }
        }

        public static User GetUserByLogin(string login)
        {
            try
            {
                using (var context = new ClockDbContext())
                    return context.Users.Include(u => u.Clocks).FirstOrDefault(u => u.Login == login);
            }
            catch (Exception)
            {
                throw new InvalidOperationException(Resources.CantGetUserError);
            }
        }

        public static User GetUserByGuid(Guid guid)
        {
            try
            {
                using (var context = new ClockDbContext())
                    return context.Users.Include(u => u.Clocks).FirstOrDefault(u => u.Id == guid);
            }
            catch (Exception)
            {
                throw new InvalidOperationException(Resources.InvalidOperationException);
            }
        }

        public static void AddUser(User user)
        {
            using (var context = new ClockDbContext())
            {
                context.Users.Add(user);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw new DbUpdateException(Resources.CantAddUserError);
                }
            }
        }

        public static void UpdateUser(User user)
        {
            using (var context = new ClockDbContext())
            {
                context.Entry(user).State = EntityState.Modified;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw new DbUpdateException(Resources.CantUpdateUserError);
                }
            }
        }
        #endregion

        #region Clocks
        public static Clock AddClock(Clock clock)
        {
            using (var context = new ClockDbContext())
            {
                clock.ClearReferences();
                context.Clocks.Add(clock);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw new DbUpdateException(Resources.CantAddClockError);
                }
            }

            return clock;
        }

        public static void SaveClock(Clock clock)
        {
            using (var context = new ClockDbContext())
            {
                context.Entry(clock).State = EntityState.Modified;
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw new DbUpdateException(Resources.CantSaveClockError);
                }
            }
        }
        
        public static void DeleteClock(Clock clock)
        {
            using (var context = new ClockDbContext())
            {
                clock.ClearReferences();
                context.Clocks.Attach(clock);
                context.Clocks.Remove(clock);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw new DbUpdateException(Resources.CantDeleteClockError);
                }
            }
        }
        #endregion
    }
}
