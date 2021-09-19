using CallCenter.Simulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Domain.Calls
{
    public class CallBroker : ICallBroker
    {
        private readonly Random _random;
        public event Action<Call> CallRegistered;
        public CallBroker()
        {
            _random = new Random();
            Work();
        }

        private void OnCallCreated(Call call)
        {
            CallRegistered?.Invoke(call);
        }

        public Task Work()
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(_random.Next(1, 6) * 1000);
                    OnCallCreated(new Call());
                }
            });
        }

    }
}
