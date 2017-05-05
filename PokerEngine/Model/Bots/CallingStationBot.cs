using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerEngine.Entities;
using PokerEngine.Enumerations;

namespace PokerEngine.Model.Bots
{
    public class CallingStationBot : Player
    {
        public CallingStationBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameAction GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            if (possibleActions.Contains(ActionType.Call))
            {
                return new GameAction
                {
                    ActionType = ActionType.Call,
                    Amount = amountToCall,
                    PlayerId = this.Id
                };
            }

            if (possibleActions.Contains(ActionType.Check))
            {
                return new GameAction
                {
                    ActionType = ActionType.Check,
                    PlayerId = this.Id
                };
            }

            return new GameAction
            {
                ActionType = ActionType.Fold,
                Amount = amountToCall,
                PlayerId = this.Id
            };
        }
    }
}
