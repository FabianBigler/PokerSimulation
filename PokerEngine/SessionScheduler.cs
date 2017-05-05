using PokerEngine.Entities;
using PokerEngine.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine
{
    public sealed class SessionScheduler
    {
        private static readonly Lazy<SessionScheduler> lazyScheduler = new Lazy<SessionScheduler>(() => new SessionScheduler());
        private IRepository<SessionEntity> sessionRepository;

        public SessionScheduler(IRepository<SessionEntity> sessionRepository)
        {            
            this.sessionRepository = sessionRepository;
            //var _service = DependencyResolver.Current
        }

        //public sealed class Singleton
        //{
        //    private static readonly Lazy<Singleton> lazy =
        //        new Lazy<Singleton>(() => new Singleton());

        //    public static Singleton Instance { get { return lazy.Value; } }

        //    private Singleton()
        //    {
        //    }
        //}

    }
}
