using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerSimulation.Game.Interfaces
{
    public interface ICanPlay
    {
        Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall);
    }
}
