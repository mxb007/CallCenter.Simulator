using CallCenter.Simulator.Domain.Agents;
using CallCenter.Simulator.Domain.Calls;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Simulator.Interfaces
{
    public interface IAgentManager
    {
        List<Agent> Agents { get; }
        IEnumerable<Agent> FreeAgents { get; }

        event Action<Agent> AgentAdded;
        event Action<Agent> AgentDeleted;

        void AddAgent(Agent agent);
        void DeleteAgent(Agent agent);
        Task ManageCalls(ConcurrentQueue<Call> AwaitingCalls);
    }
}
