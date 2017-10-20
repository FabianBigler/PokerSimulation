using System;

namespace PokerSimulation.Game.Exceptions
{
    public class IllegalActionException : Exception
    {
        public IllegalActionException(string message) : base(message)
        {
        }
    }
}
