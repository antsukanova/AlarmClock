using System;
using System.Collections.Generic;
using System.Linq;

using AlarmClock.Models;

//TODO
//finish validation hours and minutes in alarm clocks
//realization save/cancel(using Init value)
//Scroll alarms
//Add Clocks 

namespace AlarmClock.Repositories
{
    public class ClockRepository : IClockRepository
    {
        private static readonly List<Clock> Clocks = new List<Clock>();

        public bool Exists(Clock clock)
        {
            return Clocks.Any(c => c.NextTrigger == clock.NextTrigger &&
                                   c.Owner       == clock.Owner);
        }

        public List<Clock> ForUser(Guid id)
        {
            return Clocks.Where(c => c.Owner.Id == id) as List<Clock>;
        }

        public Clock Add(Clock clock)
        {
            Clocks.Add(clock);

            return clock;
        }

        public Clock Update(Clock clock)
        {
            var curr  = Clocks.Single(c => c.Id == clock.Id);
            var index = Clocks.IndexOf(curr);

            Clocks[index] = clock;

            return clock;
        }

        public Guid Delete(Guid id)
        {
            var curr = Clocks.Single(c => c.Id == id);

            Clocks.Remove(curr);

            return id;
        }
    }
}
