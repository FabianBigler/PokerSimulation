﻿using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Algorithms.TexasHoldem.OpponentModelling;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class FeatureActionAbstracter
    {
        /// <summary>
        /// Maps the action bucket to an even more abstracted feature action
        /// </summary>
        /// <param name="actionBucket"></param>
        /// <returns></returns>
        public static FeatureAction FromActionBucket(ActionBucket actionBucket)
        {
            switch (actionBucket)
            {
                case ActionBucket.Call:
                    return FeatureAction.Call;
                case ActionBucket.LowBet:
                case ActionBucket.MediumBet:
                case ActionBucket.HighBet:
                    return FeatureAction.Bet;
                case ActionBucket.Pass:
                    return FeatureAction.Pass;
                default:
                    throw new NotImplementedException("Action bucket not supported");
            }
        }


        /// <summary>
        /// Maps the real action to an abstracted action
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static FeatureAction FromActionType(ActionType actionType)
        {
            switch (actionType)
            {
                case ActionType.Call:
                    return FeatureAction.Call;
                case ActionType.Bet:
                case ActionType.Raise:
                    return FeatureAction.Bet;
                case ActionType.Check:
                case ActionType.Fold:
                    return FeatureAction.Pass;                    
                default:
                    throw new NotImplementedException("Action type not supported");
            }
        }
    }
}
