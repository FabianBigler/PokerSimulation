using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System.Collections.Generic;

namespace PokerSimulation.Core.Bots
{
    public class AlwaysMinRaiseBot : Player
    {
        public AlwaysMinRaiseBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            if (possibleActions.Contains(ActionType.Raise))
            {
                var minRaise = HeadsupGame.BigBlindSize * 2;
                if (amountToCall > HeadsupGame.BigBlindSize)
                {
                    minRaise = amountToCall * 2;
                }

                if (amountToCall == HeadsupGame.SmallBlindSize)
                {
                    //first round: small blind only has to pay $3 for min raise
                    minRaise = HeadsupGame.SmallBlindSize + HeadsupGame.BigBlindSize;
                }
                             
                return new GameActionEntity
                {
                    ActionType = ActionType.Raise,
                    Amount = minRaise,
                    PlayerId = Id
                };
            }


            if (possibleActions.Contains(ActionType.Bet))
            {
                var minBet = HeadsupGame.BigBlindSize;                
                return new GameActionEntity
                {
                    ActionType = ActionType.Bet,
                    Amount = minBet,
                    PlayerId = Id
                };
            }
            
            return new GameActionEntity
            {
                ActionType = ActionType.Call,
                Amount = amountToCall,
                PlayerId = Id
            };
        }
    }
}
