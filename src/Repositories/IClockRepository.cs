using System;
using System.Collections.Generic;

using AlarmClock.Models;

namespace AlarmClock.Repositories
{
    public interface IClockRepository
    {
        bool Exists(Clock clock);

        List<Clock> ForUser(Guid id);

        Clock Add(Clock clock);

        Clock Update(Clock clock);

        string Delete(string id);
    }
}
