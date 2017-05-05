using PokerEngine.Entities;
using PokerEngine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine
{
    public class SessionScheduler
    {
        private IRepository<SessionEntity> sessionRepository;

        public SessionScheduler(IRepository<SessionEntity> sessionRepository)
        {

        }


    }
}
