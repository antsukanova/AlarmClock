using System.Collections.Generic;
using System.Linq;

using AlarmClock.Managers;
using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> Users;

        static UserRepository() => Users = SerializationManager.DeserializeUsers() ?? new List<User>();

        public List<User> All() => Users;

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

        public User Update(User user) => Users[Users.FindIndex(u => u.Id == user.Id)] = user;
    }
}
