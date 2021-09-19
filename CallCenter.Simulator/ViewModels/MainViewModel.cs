using CallCenter.Simulator.Domain;
using CallCenter.Simulator.Domain.Agents;
using CallCenter.Simulator.Domain.Calls;
using CallCenter.Simulator.Interfaces;
using CallCenter.Simulator.Logger;
using CallCenter.Simulator.Utilities;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CallCenter.Simulator.ViewModels
{
    public class MainWindowViewModel 
    {
        public ILogger Logger { get; private set; }
        public CallCenterUnit CallCenter { get; private set; }
        public ICallBroker CallBroker { get; private set; }
        public ObservableCollection<string> Logs { get; private set; }
        public ObservableCollection<Agent> Agents { get; private set; }
        public ObservableCollection<Call> Calls { get; private set; }
        public ICommand AddAgentCommand { get; set; }
        public ICommand DeleteAgentCommand { get; set; }
        public ICommand AddCallCommand { get; set; }
        public string AgentName { get; set; }
        public Agent SelectedAgent { get; set; }

        private MainWindowViewModel()
        {
            Logs = new ObservableCollection<string>();
            Agents = new ObservableCollection<Agent>();
            Calls = new ObservableCollection<Call>();

            AddAgentCommand = new DelegateCommand<string>((agent) => AddAgent(agent)); //,(agent) => CanAddAgent(agent));
            DeleteAgentCommand = new DelegateCommand<Agent>((agent) => DeleteAgent(agent));
            AddCallCommand = new DelegateCommand(() => CallCenter.AddCall(new Call()));

            Logger = new ConsoleLogger();
            CallBroker = new CallBroker();
            CallCenter = new CallCenterUnit(CallBroker, Logger, new AgentManager(Logger));


            Logger.LogAdded += OnLogAdded;
            CallCenter.AgentManager.AgentAdded += OnAgentAdded;
            CallCenter.AgentManager.AgentDeleted += OnAgentDeleted;
            CallCenter.CallDeleted += OnCallDeleted;
            CallCenter.CallAdded += OnCallAdded;
        }

        public static MainWindowViewModel Create()
        {
            var viewModel = new MainWindowViewModel();


            viewModel.CallBroker.Work();
            viewModel.CallCenter.Work();

            viewModel.Logs.Add("Start work.");
            viewModel.CallCenter.AgentManager.AddAgent(new Agent("John", viewModel.Logger));
            viewModel.CallCenter.AgentManager.AddAgent(new Agent("Mathew", viewModel.Logger));
            viewModel.CallCenter.AgentManager.AddAgent(new Agent("Harry", viewModel.Logger));

            return viewModel;
        }

        private void OnCallAdded(Call call) => DispatcherContext.InvokeIfRequired(() => Calls.Add(call));
        private void OnCallDeleted(Call call) => DispatcherContext.InvokeIfRequired(() => Calls.Remove(call));
        private void OnAgentDeleted(Agent agent) => DispatcherContext.InvokeIfRequired(() => Agents.Remove(agent));
        private void OnAgentAdded(Agent agent) => DispatcherContext.InvokeIfRequired(() => Agents.Add(agent));
        private void OnLogAdded(string message) => DispatcherContext.InvokeIfRequired(() => Logs.Add(message));

        private void DeleteAgent(Agent agent) => CallCenter.AgentManager.DeleteAgent(agent);
        private void AddAgent(string agentName)
        {
            var agent = new Agent(agentName, Logger);
            CallCenter.AgentManager.AddAgent(agent);
        }
    }
}
