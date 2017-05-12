using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using System.Collections.Generic;

namespace PokerSimulation.Core.Model.Bots
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
