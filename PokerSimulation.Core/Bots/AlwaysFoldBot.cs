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
    public class AlwaysFoldBot : Player
    {
        public AlwaysFoldBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, int amountToCall)
        {
            ActionType selectedAction = ActionType.Check;
            if(possibleActions.Contains(ActionType.Fold))
            {
                selectedAction = ActionType.Fold;
            }

            return new GameActionEntity()
            {
                ActionType = selectedAction,
                Amount = 0,
                PlayerId = Id
            };
        }

    }
}
