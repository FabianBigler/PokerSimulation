using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Algorithms.TexasHoldem.OpponentModelling;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class FeatureActionMapper
    {
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
