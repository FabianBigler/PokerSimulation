using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Model;
using PokerSimulation.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokerSimulation.Core
{
    public class SessionScheduler : ISessionScheduler
    {        
        private IRepository<SessionEntity> sessionRepository;        
        private IPlayedHandRepository playedHandRepository;
        private ILogger logger;
        private List<Session> currentSessions;        

        public SessionScheduler(IRepository<SessionEntity> sessionRepository, IPlayedHandRepository playedHandRepository, ILogger logger)
        {            
            this.sessionRepository = sessionRepository;
            this.playedHandRepository = playedHandRepository;
            this.logger = logger;
        }

        public void StartNewSession(SessionEntity sessionEntity)
        {
            try
            {
                var session = new Session(sessionEntity, sessionRepository, playedHandRepository);
                currentSessions.Add(session);
                Task.Run(() => session.Start());
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
            }
        }

        public void PauseSession(Guid sessionId)
        {
            var session = currentSessions.FirstOrDefault(x => x.Id == sessionId);
            if(session != null)
            {
                session.Pause();
            }            
        }            

        public void ResumeSession(Guid sessionId)
        {
            var session = currentSessions.FirstOrDefault(x => x.Id == sessionId);
            session.Resume();
        }

        public void StartAllSessions()
        {
            try
            {
                currentSessions = new List<Session>();
                //also running states may be interrupted
                var sessionEntities = sessionRepository.GetAll().Where(x => x.State == SessionState.Ready ||
                                                                           x.State == SessionState.Running);
                foreach (var sessionEntity in sessionEntities)
                {
                    var session = new Session(sessionEntity, sessionRepository, playedHandRepository);
                    currentSessions.Add(session);
                    Task.Run(() => session.Start());                    
                }
            } catch (Exception ex)
            {
                logger.LogException(ex);
            }           
        }

        public Session GetSession(Guid sessionId)
        {
            return currentSessions.FirstOrDefault(x => x.Id == sessionId);
        }
    }
}
