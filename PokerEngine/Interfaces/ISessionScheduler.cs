using PokerEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Interfaces
{
    public interface ISessionScheduler
    {
        void StartNewSession(SessionEntity sessionEntity);
        void PauseSession(Guid sessionId);
        void StartAllSessions();
    }
}
