using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerEngine.Entities;
using PokerEngine.Enumerations;
using PokerEngine.Core;

namespace PokerEngine.Model.Bots
{
    public class CallingStationBot : Player
    {
        public CallingStationBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            if (possibleActions.Contains(ActionType.Call))
            {
                return new GameActionEntity
                {
                    ActionType = ActionType.Call,
                    Amount = amountToCall,
                    PlayerId = this.Id
                };
            }

            if (possibleActions.Contains(ActionType.Check))
            {
                return new GameActionEntity
                {
                    ActionType = ActionType.Check,
                    PlayerId = this.Id
                };
            }

            return new GameActionEntity
            {
                ActionType = ActionType.Fold,
                Amount = amountToCall,
                PlayerId = this.Id
            };
        }
    }
}
