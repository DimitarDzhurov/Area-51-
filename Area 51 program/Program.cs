using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area_51_program
{
    class Program
    {
        private static SemaphoreSlim elevatorSemaphore = new SemaphoreSlim(1, 1);
        public enum SecurityLevel
        {
            Confidential,
            Secret,
            TopSecret
        }
        static void Main(string[] args)
        {
            Elevator elevator = new Elevator();
            List<Agent> agents = new List<Agent>();
            agents.Add(new Agent("Agent 1", SecurityLevel.Confidential, elevator));
            agents.Add(new Agent("Agent 2", SecurityLevel.Secret, elevator));
            agents.Add(new Agent("Agent 3", SecurityLevel.TopSecret, elevator));
            foreach (Agent agent in agents)
            {
                Task.Factory.StartNew(() => agent.Simulate());
            }
            Task.Factory.StartNew(() => elevator.Run());
            Task.WaitAll(agents.Select(agent => agent.AgentTask).ToArray());
        }
    }
}

