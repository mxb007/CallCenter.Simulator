using CallCenter.Simulator.Domain.Calls;
using CallCenter.Simulator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Domain.Agents
{
    public class Agent
    {
        public string Name { get; }
        public bool IsBusy { get; private set; }

        private readonly ILogger _logger;
        private Call _currentCall;

        public Agent(string name, ILogger logger)
        {
            Name = name;
            _logger = logger;
        }

        public async Task<Call> TakeCallAsync(Call call)
        {
            if (call == null)
                return null;

            _currentCall = call;
            StartPhoneConversation();
            await Task.Delay(_currentCall.DurationInSec);
            EndPhoneConversation();
            return call;
        }

        private void StartPhoneConversation()
        {
            _currentCall.PickUpCall();
            IsBusy = true;
            _logger.Log($"Rozmowa rozpoczęcia przez {Name}");
        }

        private void EndPhoneConversation()
        {
            IsBusy = false;
            _logger.Log($"Rozmowa o długości {_currentCall.DurationInSec} została zakończona przez {Name}");
        }
    }
}
