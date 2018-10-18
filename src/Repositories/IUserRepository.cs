using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public interface IUserRepository
    {
        bool Exists(User user);

        User Add(User user);
    }
}
