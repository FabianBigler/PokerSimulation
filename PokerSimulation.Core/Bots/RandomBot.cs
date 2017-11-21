using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class RandomBot : Player
    {
        public RandomBot(PlayerEntity entity) : base(entity)
        {
        }

        public override Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall)
        {            
            var rand = new Random();
            var randIndex = rand.Next(0, possibleActions.Count);

            ActionType actionType = possibleActions[randIndex];
            int amount = 0;
            int minAmount;
            switch(actionType)
            {
                case ActionType.Bet:
                    minAmount = HeadsupGame.BigBlindSize;
                    
                    if(minAmount < ChipStack)
                    {
                        amount = rand.Next(minAmount, ChipStack);
                    } else
                    {
                        amount = rand.Next(ChipStack, minAmount);
                    }                    
                    break;
                case ActionType.Raise:
                    if (amountToCall < HeadsupGame.BigBlindSize )
                    {                                            
                        minAmount = HeadsupGame.BigBlindSize * HeadsupGame.MinAmountToBetFactor;
                    } else
                    {
                        minAmount = amountToCall * HeadsupGame.MinAmountToBetFactor;
                    }

                    if (minAmount < ChipStack)
                    {
                        amount = rand.Next(minAmount, ChipStack);
                    }
                    else
                    {
                        amount = rand.Next(ChipStack, minAmount);
                    }
                    break;
                case ActionType.Call:
                    amount = amountToCall;
                    break;
            }

            if(ChipStack < amount) amount = this.ChipStack;                            
                                  
            return Task.FromResult<GameActionEntity>(
                new GameActionEntity
                {
                    PlayerId = Id,
                    ActionType = possibleActions[randIndex],
                    Amount = amount
                });         
        }
    }
}
