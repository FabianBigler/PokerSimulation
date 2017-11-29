using PokerSimulation.Core.Bots;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;

namespace PokerSimulation.Core.Model
{
    public class PlayerFactory
    {
        public Player GetPlayer(PlayerEntity playerEntity)
        {
            switch(playerEntity.Type)
            {
                case PlayerType.AlwaysMinRaise:
                    return new AlwaysMinRaiseBot(playerEntity);
                case PlayerType.CallingStation:
                    return new CallingStationBot(playerEntity);
                case PlayerType.Random:
                    return new RandomBot(playerEntity);
                case PlayerType.CounterFactualRegret:
                    return new MinimalRegretBot(playerEntity);
                case PlayerType.AlwaysFold:
                    return new AlwaysFoldBot(playerEntity);
                case PlayerType.OpponentModelling:
                    return new OpponentModellingBot(playerEntity);
                case PlayerType.Human:
                    return new HumanPlayer(playerEntity);
                case PlayerType.TightAggressive:
                    return new TightAggressiveBot(playerEntity);
                case PlayerType.TightPassive:
                    return new TightPassiveBot(playerEntity);
                case PlayerType.LooseAggressive:
                    return new LooseAggressiveBot(playerEntity);
                case PlayerType.LoosePassive:
                    return new LoosePassiveBot(playerEntity);
                default:
                    throw new NotImplementedException("PlayerType not implemented in PlayerFactory!");                    
            }
        }
    }    
}
