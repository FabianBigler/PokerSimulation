using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Core.Model.Bots
{
    public class RandomBot : Player
    {
        public RandomBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {            
            var rand = new Random();
            var randIndex = rand.Next(0, possibleActions.Count);

            var actionType = possibleActions[randIndex];
            var amount = 0;
            switch(actionType)
            {
                case ActionType.Bet:
                    amount = HeadsupGame.BigBlindSize;
                    break;
                case ActionType.Raise:
                    if (amountToCall < HeadsupGame.BigBlindSize )
                    {
                        amount = HeadsupGame.BigBlindSize * HeadsupGame.MinAmountToBetFactor;
                    } else
                    {
                        amount = amountToCall * HeadsupGame.MinAmountToBetFactor;
                    }                    
                    break;
                case ActionType.Call:
                    amount = amountToCall;
                    break;
            }

            if(ChipStack < amount) amount = this.ChipStack;                            
                      
            var action = new GameActionEntity
            {
                PlayerId = Id,
                ActionType = possibleActions[randIndex],
                Amount = amount
            };            

            return action;         
        }
    }
}
