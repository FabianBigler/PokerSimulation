using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;

namespace PokerSimulation.Core.Bots
{
    public class OpponentModellingBot : Player
    {
        public OpponentModellingBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, int amountToCall)
        {

            throw new NotImplementedException();     
        }
    }
}
