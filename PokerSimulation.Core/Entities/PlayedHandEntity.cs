using PokerSimulation.Core.Model;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Core.Entities
{
    public class PlayedHandEntity
    {
        public Guid Id { get; set; }
        public Guid WinnerId { get; set; }
        public int AmountWon { get; set; }
        public string HoleCards1 { get; set; }
        public string HoleCards2 { get; set; }
        public string Board { get; set; }

        public int PotSize { get; set; }              
          
        public DateTime Timestamp { get; set; }
        public Guid SessionId { get; set; }        

        public List<GameActionEntity> Actions { get; set; }        

        public PlayedHandEntity()
        {
            Actions = new List<GameActionEntity>();
        }
    }
}
