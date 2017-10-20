using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Interfaces
{
    public interface IPlayedHandRepository : IRepository<PlayedHandEntity>
    {
        IEnumerable<PlayedHandEntity> GetAllBySessionId(Guid sessionId);
    }
}
