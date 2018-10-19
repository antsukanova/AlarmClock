using System.Collections.Generic;
using System.Linq;
using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> Users = new List<User>();

        public User Find(string emailOrLogin)
        {
            return Users.FirstOrDefault(u => u.Email == emailOrLogin || u.Login == emailOrLogin);
        }

        public bool Exists(User user)
        {
            return Users.Any(u => u.Login == user.Login || u.Email == user.Email);
        }

        public User Add(User user)
        {
            Users.Add(user);

            return user;
        }
    }
}
