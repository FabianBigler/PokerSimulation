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
                default:
                    throw new NotImplementedException("PlayerType not implemented in PlayerFactory!");                    
            }
        }
    }    
}
