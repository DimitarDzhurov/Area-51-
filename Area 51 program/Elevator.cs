using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Area_51_program.Program;

namespace Area_51_program
{
    class Elevator
    {
        private static SemaphoreSlim elevatorSemaphore = new SemaphoreSlim(1, 1);
        private int currentFloor;
        private bool isMoving;
        public bool IsMoving { get; internal set; }
        public async Task MoveToFloor(int targetFloor)
        {
            await Task.Run(() =>
            {
                isMoving = true;
                Console.WriteLine($"Elevator moving from floor {currentFloor} to floor {targetFloor}.");
                Thread.Sleep(Math.Abs(targetFloor - currentFloor) * 1000); // Simulating elevator movement
                currentFloor = targetFloor;
                Console.WriteLine($"Elevator reached floor {currentFloor}.");
                isMoving = false;
            });
        }
        public async Task<bool> OpenDoor(SecurityLevel agentSecurityLevel)
        {
            await elevatorSemaphore.WaitAsync();
            try
            {
                if (isMoving)
                {
                    Console.WriteLine("Elevator is still moving. Door cannot be opened.");
                    return false;
                }
                switch (currentFloor)
                {
                    case 0: // Ground floor
                        return true; // All agents can access ground floor
                    case 1: // Secret floor
                        return agentSecurityLevel >= SecurityLevel.Secret;
                    case 2: // Top-secret floor
                        return agentSecurityLevel >= SecurityLevel.TopSecret;
                    case 3: // Experimental floor
                        return agentSecurityLevel >= SecurityLevel.TopSecret;
                    default:
                        return false;
                }
            }
            finally
            {
                elevatorSemaphore.Release();
            }
        }
        internal void Run()
        {
            throw new NotImplementedException();
        }
    }
}
