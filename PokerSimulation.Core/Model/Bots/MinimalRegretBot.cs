using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Model.Bots
{
    public class MinimalRegretBot : Player
    {
        public MinimalRegretBot(PlayerEntity entity) : base(entity)
        {

        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            throw new NotImplementedException();

            //var rand = new Random();
            //var randIndex = rand.Next(0, possibleActions.Count);
            //var actionType = possibleActions[randIndex];
            //var amount = 0;
            //switch (actionType)
            //{
            //    case ActionType.Bet:
            //        amount = HeadsupGame.BigBlindSize;
            //        break;
            //    case ActionType.Raise:
            //        if (amountToCall < HeadsupGame.BigBlindSize)
            //        {
            //            amount = HeadsupGame.BigBlindSize * HeadsupGame.MinAmountToBetFactor;
            //        }
            //        else
            //        {
            //            amount = amountToCall * HeadsupGame.MinAmountToBetFactor;
            //        }
            //        break;
            //    case ActionType.Call:
            //        amount = amountToCall;
            //        break;
            //}

            //if (ChipStack < amount) amount = this.ChipStack;

            //var action = new GameActionEntity
            //{
            //    PlayerId = Id,
            //    ActionType = possibleActions[randIndex],
            //    Amount = amount
            //};

            //return action;
        }
    }
}
