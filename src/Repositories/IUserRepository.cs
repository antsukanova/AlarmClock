using System.Collections.Generic;

using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public interface IUserRepository
    {
        List<User> All();

        User Find(string emailOrLogin);

        bool Exists(User user);

        User Add(User user);

        User Update(User user);
    }
}
