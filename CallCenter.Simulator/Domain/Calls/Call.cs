using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Domain.Calls
{
    public class Call
    {
        public Guid Id { get; } = Guid.NewGuid();
        public TimeSpan DurationInSec { get; private set; }

        public void PickUpCall()
        {
            DurationInSec = TimeSpan.FromSeconds(new Random().Next(1, 10));
        }
    }
}
