using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public interface IUserRepository
    {
        User Find(string emailOrLogin);

        bool Exists(User user);

        User Add(User user);

        User UpdateLastVisited(User user);
    }
}
