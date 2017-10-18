using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;

namespace PokerSimulation.Core.Model.Bots
{
    public class CounterFactualRegretBot : Player
    {
        public CounterFactualRegretBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {

            return null;

        }
    }
}
