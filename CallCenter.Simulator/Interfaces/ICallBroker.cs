using CallCenter.Simulator.Domain.Calls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Interfaces
{
    public interface ICallBroker
    {
        event Action<Call> CallRegistered;

        Task Work();
    }
}
