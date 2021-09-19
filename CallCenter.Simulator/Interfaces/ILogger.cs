using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        List<string> Logs { get; }
        event Action<string> LogAdded;
    }
}
