using System;

namespace PokerSimulation.Game.Exceptions
{
    public class InvalidCardCountException : Exception
    {
        public InvalidCardCountException(string message) : base(message)
        {
        }
    }
}
