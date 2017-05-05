using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerEngine.Entities;
using PokerEngine.Enumerations;

namespace PokerEngine.Model.Bots
{
    public class RandomBot : Player
    {
        public RandomBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameAction GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            this.ChipStack -= amountToCall;
            var rand = new Random();
            var randIndex = rand.Next(0, possibleActions.Count - 1);

            var action = new GameAction
            {
                PlayerId = Id,
                ActionType = possibleActions[randIndex],
                Amount = amountToCall                
            };            

            return action;         
        }
    }
}
