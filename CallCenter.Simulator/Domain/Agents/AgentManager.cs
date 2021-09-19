using CallCenter.Simulator.Domain.Calls;
using CallCenter.Simulator.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Domain.Agents
{
    public class AgentManager : IAgentManager
    {
        private static object _lock = new object();
        private List<Agent> _agents = new List<Agent>();
        public List<Agent> Agents
        {
            get
            {
                lock (_lock)
                {
                    return _agents;
                }
            }
            private set
            {
                _agents = value;
            }
        }
        public IEnumerable<Agent> FreeAgents => Agents.Where(x => !x.IsBusy);

        private readonly ILogger _logger;

        public AgentManager(ILogger logger)
        {
            _logger = logger;
        }

        public void AddAgent(Agent agent)
        {
            Agents.Add(agent);
            _logger.Log($"Agent {agent.Name} został dodany");
            OnAgentAdded(agent);
        }

        public void DeleteAgent(Agent agent)
        {
            Agents.Remove(agent);
            _logger.Log($"Agent {agent.Name} został usunięty");
            OnAgentDeleted(agent);
        }
        private void OnAgentAdded(Agent agent)
        {
            AgentAdded?.Invoke(agent);
        }

        private void OnAgentDeleted(Agent agent)
        {
            AgentDeleted?.Invoke(agent);
        }

        public event Action<Agent> AgentAdded;
        public event Action<Agent> AgentDeleted;

        public Task ManageCalls(ConcurrentQueue<Call> AwaitingCalls)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    while (AwaitingCalls.Any())
                    {
                        lock (AwaitingCalls)
                        {
                            var freeAgent = FreeAgents.FirstOrDefault();
                            Call call = null;

                            if (AwaitingCalls.Count > 0 && freeAgent != null)
                            {

                                if (AwaitingCalls.Any())
                                    AwaitingCalls.TryDequeue(out call);

                                if (call != null)
                                {
                                    freeAgent.TakeCallAsync(call).Start();
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
