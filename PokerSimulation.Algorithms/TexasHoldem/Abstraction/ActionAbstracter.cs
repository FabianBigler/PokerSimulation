using PokerSimulation.Game;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public class ActionAbstracter
    {     
        public static int GetBetSize(ActionBucket action, int amountToCall, int potSize)
        {
            int currentMoneyInPot = (potSize - amountToCall) / 2;
            int betSize = 0;
            if (amountToCall == 0)
            {
                switch (action)
                {
                    case ActionBucket.HighBet:
                        betSize = HeadsupGame.StackSize - currentMoneyInPot;
                        break;
                    case ActionBucket.MediumBet:
                        betSize = potSize;
                        break;
                    case ActionBucket.LowBet:
                        betSize = potSize / 2;
                        break;
                }
            }
            else
            {
                int betSizeFactor = 0;
                if (amountToCall == HeadsupGame.SmallBlindSize)
                {                    
                    betSizeFactor = HeadsupGame.BigBlindSize;
                }
                else
                {
                    betSizeFactor = amountToCall;
                }

                switch (action)
                {
                    case ActionBucket.HighBet:
                        betSize = HeadsupGame.StackSize - currentMoneyInPot;
                        break;
                    case ActionBucket.MediumBet:
                        betSize = betSizeFactor * 3;
                        break;
                    case ActionBucket.LowBet:
                        betSize = betSizeFactor * 2;
                        break;
                }
            }

            if ((betSize + currentMoneyInPot) > HeadsupGame.StackSize)
            {
                betSize = HeadsupGame.StackSize - currentMoneyInPot;
            }

            return betSize;
        }

        //public static ActionBucket ToActionBucket(ActionType action, int amountToCall, int potSize)
        //{            
        //    switch(action)
        //    {
        //        case ActionType.Call:
        //            return ActionBucket.Call;
        //        case ActionType.Check:
        //        case ActionType.Fold:
        //            return ActionBucket.Pass;
        //        case ActionType.Bet:

        //            //TODO @fb                    

        //        //-Bet ½ des Pots (in diesem Fall $4)
        //        //-Bet ¾ des Pots (in diesem Fall $6)
        //        //-Bet Grösse des Pots(in diesem Fall $8)
        //        //- All - In(die verbleibenden $194)

        //            break;
        //        case ActionType.Raise:
        //            //TODO @fb
        //            break;
        //    }

        //    return ActionBucket.Pass;
        //}

        //public static GameAction ToAction(ActionBucket bucket, int potSize, int amountToCall)
        //{
        //    var action = new GameAction();
        //    switch(bucket)
        //    {
        //        case ActionBucket.Call:

        //            break;              
        //        case ActionBucket.HighBet:

        //            break;
        //        case ActionBucket.LowBet:

        //            break;
        //        case ActionBucket.MediumBet:

        //            break;
        //        case ActionBucket.Pass:

        //            break;                
        //    }

        //    return action;
        //}
    }
}
