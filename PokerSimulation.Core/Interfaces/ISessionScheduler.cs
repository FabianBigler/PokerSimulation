using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Model;
using System;

namespace PokerSimulation.Core.Interfaces
{
    public interface ISessionScheduler
    {
        void StartNewSession(SessionEntity sessionEntity);
        void PauseSession(Guid sessionId);
        void ResumeSession(Guid sessionId);
        void StartAllSessions();
        Session GetSession(Guid sessionId);  
    }
}
