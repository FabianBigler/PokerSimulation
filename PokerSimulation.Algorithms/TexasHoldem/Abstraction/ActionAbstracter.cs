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

        public static ActionBucket MapToBucket(ActionType action, int amountToCall, int potSize, int amount)
        {
            switch (action)
            {
                case ActionType.Call:
                    return ActionBucket.Call;
                case ActionType.Check:
                case ActionType.Fold:
                    return ActionBucket.Pass;
                case ActionType.Bet:
                case ActionType.Raise:
                    int low = GetBetSize(ActionBucket.LowBet, amountToCall, potSize);
                    int medium = GetBetSize(ActionBucket.MediumBet, amountToCall, potSize);
                    int high = GetBetSize(ActionBucket.HighBet, amountToCall, potSize);

                    if (amount <= low) return ActionBucket.LowBet;
                    if (amount >= high) return ActionBucket.HighBet;
                    if (amount == medium) return ActionBucket.MediumBet;

                    //the amount's nearest neighbours are LowBet and MediumBet                                     
                    if (amount > low && amount < medium)
                    {
                        return getBucketByDistanceProbability(amount, low, medium, ActionBucket.LowBet, ActionBucket.MediumBet);         
                    }
                    //the amount's nearest neighbours are MediumBet and HighBet
                    if (amount > medium && amount < high)
                    {                        
                        return getBucketByDistanceProbability(amount, medium, high, ActionBucket.MediumBet, ActionBucket.HighBet);
                    }
                    break;
            }

            return ActionBucket.Pass;
        }

        public static ActionType MapToAction(ActionBucket action, int amountToCall)
        {
            switch (action)
            {
                case ActionBucket.Call:
                    return ActionType.Call;
                case ActionBucket.Pass:
                    if(amountToCall == 0)
                    {
                        return ActionType.Check;
                    } else
                    {
                        return ActionType.Fold;
                    }
                case ActionBucket.LowBet:
                case ActionBucket.HighBet:
                case ActionBucket.MediumBet:
                    if(amountToCall == 0)
                    {
                        return ActionType.Bet;
                    } else
                    {
                        return ActionType.Raise;
                    }                                                             
            }

            return ActionType.Illegal;
        }

        /// <summary>
        /// Minimise exploitability by mapping the bucket based on the distance to the nearest neighbours
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <param name="lowBucket"></param>
        /// <param name="highBucket"></param>
        /// <returns></returns>
        private static ActionBucket getBucketByDistanceProbability(int amount, int low, int high, ActionBucket lowBucket, ActionBucket highBucket)
        {
            var random = new Random();
            int distanceAmountToLow = amount - low;
            int distanceLowToHigh = high - low;
            decimal probabilityHigh = (decimal)(distanceAmountToLow / distanceLowToHigh) * 100;
            int probabilityHighPercentage = (int)Math.Round(probabilityHigh);
            int randomValue = random.Next(1, 100);
            if (randomValue <= probabilityHighPercentage)
            {
                return highBucket;
            }
            else
            {
                return lowBucket;
            }
        }       
    }
}
