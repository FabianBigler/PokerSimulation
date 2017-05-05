using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerEngine.Enumerations;
using PokerEngine.Entities;

namespace TestPokerEngine
{
    public class AlwaysMinRaiseBot : Player
    {
        public AlwaysMinRaiseBot(PlayerEntity entity) : base(entity)
        {
        }

        public override GameAction GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
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
                             
                return new GameAction
                {
                    ActionType = ActionType.Raise,
                    Amount = minRaise,
                    PlayerId = Id
                };
            }


            if (possibleActions.Contains(ActionType.Bet))
            {
                var minBet = HeadsupGame.BigBlindSize;                
                return new GameAction
                {
                    ActionType = ActionType.Raise,
                    Amount = minBet,
                    PlayerId = Id
                };
            }
            
            return new GameAction
            {
                ActionType = ActionType.Call,
                Amount = amountToCall,
                PlayerId = Id
            };
        }
    }
}
