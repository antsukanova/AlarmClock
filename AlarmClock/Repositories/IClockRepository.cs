using System;
using System.Collections.Generic;

using AlarmClock.DBModels;

namespace AlarmClock.Repositories
{
    public interface IClockRepository
    {
        List<Clock> All();

        bool Exists(Clock clock);

        List<Clock> ForUser(Guid id);

        Clock Add(Clock clock);

        Clock Update(Clock clock);

        Guid Delete(Guid id);
    }
}
