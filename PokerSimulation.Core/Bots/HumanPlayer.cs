using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game;

namespace PokerSimulation.Core.Bots
{
    public class HumanPlayer : Player
    {
        

        public HumanPlayer(PlayerEntity entity) : base(entity)
        {

        }

        public override async Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall)
        {
            GameActionEntity action = null;
            bool actionReceived = false;            

            while(!actionReceived)
            {
                action = base.OnActionRequired(possibleActions, amountToCall);
                if (action != null)
                {
                    actionReceived = true;                    
                } else
                {
                    await Task.Delay(1000);
                }
            }

            return await Task.FromResult<GameActionEntity>(action);
        }
    }
}
