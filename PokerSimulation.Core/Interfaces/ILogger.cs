using System;

namespace PokerSimulation.Core.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void LogException(Exception ex);
    }
}
