using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Model;
using System.Collections.Generic;

namespace PokerSimulation.Core.Interfaces
{
    public interface ICanPlay
    {
        GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall);
    }
}
