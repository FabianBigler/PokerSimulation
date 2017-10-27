using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using System.Collections.Generic;

namespace PokerSimulation.Game.Interfaces
{
    public interface ICanPlay
    {
        GameActionEntity GetAction(List<ActionType> possibleActions, int amountToCall);
    }
}
