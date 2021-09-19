using CallCenter.Simulator.Domain.Agents;
using CallCenter.Simulator.Domain.Calls;
using CallCenter.Simulator.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Domain
{
    public class CallCenterUnit : IDisposable
    {
        public ConcurrentQueue<Call> AwaitingCalls { get; private set; } = new ConcurrentQueue<Call>();
        public List<Agent> Agents { get; } = new List<Agent>();
        public IAgentManager AgentManager { get; set; }

        private readonly ICallBroker _callBroker;
        private readonly ILogger _logger;

        public CallCenterUnit(ICallBroker callBroker, ILogger logger, IAgentManager agentManager)
        {
            AgentManager = agentManager;
            _callBroker = callBroker;
            _logger = logger;
            callBroker.CallRegistered += OnCallRegistered;
        }

        private void OnCallAdded(Call call) => CallAdded?.Invoke(call);
        //private void OnCallDeleted(Call call) => CallDeleted?.Invoke(call);
        private void OnCallRegistered(Call incommingCall) => AddCall(incommingCall);

        public void AddCall(Call call)
        {
            AwaitingCalls.Enqueue(call);
            OnCallAdded(call);
            _logger.Log($"Połączenie {call.Id} zostało zarejestrowane.");
        }

        public Task Work()
        {
            return AgentManager.ManageCalls(AwaitingCalls);
        }

        public void Dispose()
        {
            _callBroker.CallRegistered -= OnCallRegistered;
        }

        public event Action<Call> CallAdded;
        public event Action<Call> CallDeleted;

    }
}
