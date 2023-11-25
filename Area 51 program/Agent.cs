using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Area_51_program.Program;

namespace Area_51_program
{
    class Agent
    {
        private readonly string name;
        private readonly SecurityLevel securityLevel;
        private readonly Elevator elevator;
        private string v;
        private Program.SecurityLevel confidential;
        public Task AgentTask { get; private set; }
        public Agent(string name, SecurityLevel securityLevel, Elevator elevator)
        {
            this.name = name;
            this.securityLevel = securityLevel;
            this.elevator = elevator;
        }
        public void Simulate()
        {
            Random random = new Random();
            int currentFloor = 0;
            while (true)
            {
                int targetFloor = random.Next(0, 4);
                Console.WriteLine($"{name} is on floor {currentFloor} and wants to go to floor {targetFloor}.");
                // Call the elevator
                elevator.MoveToFloor(currentFloor).Wait();
                Console.WriteLine($"{name} pressed the elevator button on floor {currentFloor}.");
                // Wait for the elevator to arrive
                while (elevator.IsMoving) { }
                // Enter the elevator
                elevator.OpenDoor(securityLevel).ContinueWith(result =>
                {
                    if (result.Result)
                    {
                        Console.WriteLine($"{name} entered the elevator on floor {currentFloor}.");
                        currentFloor = targetFloor;

                        // Simulate time inside the elevator
                        Thread.Sleep(2000);

                        // Exit the elevator
                        elevator.OpenDoor(securityLevel).ContinueWith(exitResult =>
                        {
                            if (exitResult.Result)
                                Console.WriteLine($"{name} exited the elevator on floor {currentFloor}.");
                        }).Wait();
                    }
                    else
                    {
                        Console.WriteLine($"{name} doesn't have the required security level to enter the elevator on floor {currentFloor}.");
                    }
                }).Wait();
                // Simulate time outside the elevator
                Thread.Sleep(random.Next(1000, 5000));
            }
        }
    }
}
