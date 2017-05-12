using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Model.Bots;
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
                default:
                    throw new NotImplementedException("PlayerType not implemented in PlayerFactory!");                    
            }
        }
    }    
}
