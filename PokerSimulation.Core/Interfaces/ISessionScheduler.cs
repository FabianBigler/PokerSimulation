using PokerSimulation.Core.Entities;
using System;

namespace PokerSimulation.Core.Interfaces
{
    public interface ISessionScheduler
    {
        void StartNewSession(SessionEntity sessionEntity);
        void PauseSession(Guid sessionId);
        void StartAllSessions();
    }
}
