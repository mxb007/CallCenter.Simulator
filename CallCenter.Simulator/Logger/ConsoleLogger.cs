using CallCenter.Simulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Logger
{
    public class ConsoleLogger : ILogger
    {
        public List<string> Logs { get; private set; }

        public event Action<string> LogAdded;

        public ConsoleLogger()
        {
            Logs = new List<string>();
        }

        public void Log(string message)
        {
            Logs.Add(message);
            OnLogAdded(message);
        }

        protected virtual void OnLogAdded(string message)
        {
            LogAdded?.Invoke(message);
        }

    }
}
