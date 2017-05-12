using System;

namespace PokerSimulation.Core.Exceptions
{
    public class InvalidCardCountException : Exception
    {
        public InvalidCardCountException(string message) : base(message)
        {
        }
    }
}
