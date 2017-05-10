using PokerEngine.Entities;
using PokerEngine.Enumerations;
using PokerEngine.Model.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
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
