using System;

namespace PokerSimulation.Core.Exceptions
{
    public class IllegalActionException : Exception
    {
        public IllegalActionException(string message) : base(message)
        {
        }
    }
}
