using PokerEngine.Entities;
using PokerEngine.Interfaces;
using PokerEngine.Model;
using PokerEngine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Core
{
    public class SessionScheduler : ISessionScheduler
    {        
        private IRepository<SessionEntity> sessionRepository;        
        private IRepository<PlayedHandEntity> playedHandRepository;
        private ILogger logger;
        private List<Session> currentSessions;        

        public SessionScheduler(IRepository<SessionEntity> sessionRepository, IRepository<PlayedHandEntity> playedHandRepository, ILogger logger)
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
            session.Pause();
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

        //private void startSessions()
        //{
        //    var tasks = new List<Task>();
        //    foreach (var session in currentSessions)
        //    {
        //        Task.Run(() => session.Start());
        //        tasks.Add(Task.Run(() => session.Start()));
        //    }
        //    Task.WaitAll(tasks.ToArray());            
        //}
    }
}
